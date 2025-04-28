using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Services;

public class UserDbService : IUserDbService
{   
    private readonly AppDbContext _dbContext;

    public UserDbService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _dbContext.Users
            .Include(u => u.Address)
            .Include(u => u.Employments)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
    

    public async Task UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task VerifyUserEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null)
        {
            throw new Exception("User with this email already exists.");
        }
    }
}