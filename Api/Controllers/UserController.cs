using System;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(AppDbContext dbContext) : ControllerBase
{
    AppDbContext _dbContext = dbContext;

    [HttpPost]
    public ActionResult<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors);
        }

        var user = request.ToUser();

        if (_dbContext.Users.Any(u => u.Email == user.Email))
        {
            return Conflict(new { Message = "User with this email already exists." });
        }

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToCreateUserResponse());
    }

    [HttpGet("{id}")]
    public ActionResult<GetUserResponse> GetUser(int id)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null) return NotFound();

        return Ok(user.ToGetUserByIdResponse());
    }

    [HttpPut("{id}")]
    public ActionResult<UpdateUserResponse> UpdateUser(int id, UpdateUserRequest request)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null) return NotFound();

        var validator = new UpdateUserRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors);
        }

        if (_dbContext.Users.Any(u => u.Email == request.Email && u.Id != id))
        {
            return Conflict(new { Message = "User with this email already exists." });
        }

        user.UpdateUser(request);
        _dbContext.SaveChanges();
    
        return Ok(user.ToUpdateUserResponse());
    }

    [HttpPost("{userId}/employments")]
    public ActionResult<CreateUserEmploymentResponse> CreateUserEmployment(int userId, CreateUserEmploymentRequest request)
    {
        var user = _dbContext.Users
            .Include(u => u.Employments)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null) return NotFound();

        var validator = new CreateUserEmploymentRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors);
        }

        var employment = request.ToEmployment();
        user.AddEmployment(employment);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, employment.ToCreateUserEmploymentResponse());
    }

    [HttpDelete("{userId}/employments/{id}")]
    public ActionResult DeleteUserEmployment(int userId, int id)
    {
        var user = _dbContext.Users
            .Include(u => u.Employments)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null) return NotFound();

        var employment = user.Employments.FirstOrDefault(e => e.Id == id);
        if (employment == null) return NotFound();

        user.RemoveEmployment(employment);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
