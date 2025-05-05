using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;

namespace Core.Services.Users
{
    [AutoRegister]
    public class CreateUserService : ICreateUserService
    {
        private readonly IUpdateUserService _updateUserService;
        private readonly IIdObjectFactory<User> _userFactory;
        private readonly IUserRepository _userRepository;

        public CreateUserService(IIdObjectFactory<User> userFactory, IUserRepository userRepository, IUpdateUserService updateUserService)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _updateUserService = updateUserService;
        }

        public User Create(Guid id, string name, string email, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            try
            {
                // Check if the ID already exists in the repository
                var existingId = _userRepository.Get(id);

                if (existingId != null)
                {
                    throw new InvalidOperationException($"User with ID {id} already exists.");
                }

                // Create a new user instance
                var user = _userFactory.Create(id);

                // Update the user details
                _updateUserService.Update(user, name, email, type, annualSalary, tags);

                // Save the user to the repository
                _userRepository.Save(user);

                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{ex.Message}");
            }

        }
    }
}