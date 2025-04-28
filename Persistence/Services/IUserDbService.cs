using System;
using Domain;

namespace Persistence.Services;

public interface IUserDbService
{   
    Task<User?> GetUserByIdAsync(int id);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task VerifyUserEmailAsync(string email);
}
