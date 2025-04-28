using Application;
using Microsoft.AspNetCore.Mvc;
using Persistence.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserDbService userDbService) : ControllerBase
{
    IUserDbService _userDbService = userDbService;

    [HttpGet("{id}")]
    public async Task<ActionResult<GetUserResponse>> GetUser(int id)
    {
        var user = await _userDbService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(user.ToGetUserByIdResponse());
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors);
        }

        var user = request.ToUser();

        try
        {
            await _userDbService.VerifyUserEmailAsync(user.Email);
            await _userDbService.CreateUserAsync(user);
        }
        catch (Exception ex)
        {
            return Conflict(new { ex.Message });  
        }
        
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToCreateUserResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(int id, UpdateUserRequest request)
    {
        var user = await _userDbService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        var validator = new UpdateUserRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _userDbService.VerifyUserEmailAsync(request.Email);
            user.UpdateUser(request);
            await _userDbService.UpdateUserAsync(user);
        }
        catch (Exception ex)
        {
            return Conflict(new { ex.Message });  
        }


        return Ok(user.ToUpdateUserResponse());
    }

    [HttpPost("{userId}/employments")]
    public async Task<ActionResult<CreateUserEmploymentResponse>> CreateUserEmployment(int userId, CreateUserEmploymentRequest request)
    {
        var user = await _userDbService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        var validator = new CreateUserEmploymentRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        { 
            return BadRequest(validationResult.Errors);
        }

        var employment = request.ToEmployment();
        user.AddEmployment(employment);
        await _userDbService.UpdateUserAsync(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, employment.ToCreateUserEmploymentResponse());
    }

    [HttpDelete("{userId}/employments/{id}")]
    public async Task<ActionResult> DeleteUserEmployment(int userId, int id)
    {
        var user = await _userDbService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        var employment = user.Employments.FirstOrDefault(e => e.Id == id);
        if (employment == null) return NotFound();

        user.RemoveEmployment(employment);
        await _userDbService.UpdateUserAsync(user);

        return NoContent();
    }
}
