using Moq;
using Api.Controllers;
using Persistence.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Application;
using FluentValidation;
using FluentValidation.Results;
namespace Api.Tests;

public class UserControllerTests
{
    private readonly UserController _sut;
    private readonly Mock<IUserDbService> _userDbServiceMock;

    public UserControllerTests()
    {
        _userDbServiceMock = new Mock<IUserDbService>();
        _sut = new UserController(_userDbServiceMock.Object);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var user = new User() 
        { 
            Id = userId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = null
        };

        _userDbServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var actionResult = await _sut.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var response = Assert.IsType<GetUserResponse>(okResult.Value);
        Assert.Equal(userId, response.Id);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _userDbServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(null as User);

        // Act
        var actionResult = await _sut.GetUser(userId);

        // Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreated_WhenUserIsValid()
    {
        // Arrange
        var request = new CreateUserRequest("John", "Doe", "john.doe@example.com", null, []);

        var user = new User() 
        { 
            Id = 1, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = null,
            Employments = []
        };

        _userDbServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()));

        // Act
        var actionResult = await _sut.CreateUser(request);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var response = Assert.IsType<CreateUserResponse>(createdResult.Value);
        Assert.Equal(user.FirstName, response.FirstName);
        Assert.Equal(user.LastName, response.LastName);
        Assert.Equal(user.Email, response.Email);
        Assert.Equal(user.Address, response.Address?.ToAddress());
        Assert.Empty(response.Employments);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new CreateUserRequest("John", "Doe", "john.doe@example.com", null, []);

        var user = new User() 
        { 
            Id = 1, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = null,
            Employments = []
        };

        _userDbServiceMock.Setup(x => x.VerifyUserEmailAsync(user.Email))
            .ThrowsAsync(new Exception("User with this email already exists."));

        // Act
        var actionResult = await _sut.CreateUser(request);
        var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult.Result);
        Assert.Equal("{ Message = User with this email already exists. }", conflictResult?.Value?.ToString());
    }

    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest_WhenEmploymentIsInvalid()
    {
        // Arrange
        var invalidUserEmployment = new UserEmploymentRequest("Company", 1, DateTime.Now.AddYears(1), DateTime.Now);
        var request = new CreateUserRequest("John", "Doe", "john.doe@example.com", null, [invalidUserEmployment]);

        // Act
        var actionResult = await _sut.CreateUser(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var errors = Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        Assert.Equal("End date must be greater than or equal to start date.", errors.First().ErrorMessage);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest_WhenAddressIsInvalid()
    {
        // Arrange
        var invalidAddress = new UserAddress("", "", "");
        var request = new CreateUserRequest("John", "Doe", "john.doe@example.com", invalidAddress, []);
        _userDbServiceMock.Setup(x => x.VerifyUserEmailAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("User with this email already exists."));
        // Act
        var actionResult = await _sut.CreateUser(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest_WhenMandatoryFieldsIsNull()
    {
        // Arrange
        var request = new CreateUserRequest("", "", "", null, []);

        // Act
        var actionResult = await _sut.CreateUser(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var errors = Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        Assert.Equal("First name is required.", errors[0].ErrorMessage);
        Assert.Equal("Last name is required.", errors[1].ErrorMessage);
        Assert.Equal("'Email' must not be empty.", errors[2].ErrorMessage);
        Assert.Equal("A valid email address is required.", errors[3].ErrorMessage);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var userId = 1;
        var request = new UpdateUserRequest("John", "Doe", "john.doe@example.com", null);

        var user = new User() 
        { 
            Id = userId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example2.com",
            Address = null,
            Employments = []
        };

        _userDbServiceMock
            .Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        _userDbServiceMock
            .Setup(x => x.VerifyUserEmailAsync(request.Email))
            .ThrowsAsync(new Exception("User with this email already exists."));

        // Act
        var actionResult = await _sut.UpdateUser(userId, request);
        var conflictResult = Assert.IsType<ConflictObjectResult>(actionResult.Result);
        Assert.Equal("{ Message = User with this email already exists. }", conflictResult?.Value?.ToString());
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk_WhenEmailIsNotModified()
    {
        // Arrange
        var userId = 1;
        var email = "john.doe@example.com";
        var request = new UpdateUserRequest("John", "Doe", email, null);

        var user = new User() 
        { 
            Id = userId, 
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            Address = null,
            Employments = []
        };

        _userDbServiceMock
            .Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var actionResult = await _sut.UpdateUser(userId, request);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.IsType<UpdateUserResponse>(okResult.Value);
    }

    [Fact]
    public async Task CreateUserEmployment_ShouldReturnBadRequest_WhenEmploymentIsInvalid()
    {
        // Arrange
        var userId = 1;
        var request = new CreateUserEmploymentRequest(userId, "Company", 1, DateTime.Now.AddYears(1), DateTime.Now);

        var user = new User() 
        { 
            Id = userId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = null,
            Employments = []
        };

        _userDbServiceMock
            .Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var actionResult = await _sut.CreateUserEmployment(userId, request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var errors = Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        Assert.Equal("End date must be greater than or equal to start date.", errors.First().ErrorMessage);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeleted()
    {
        // Arrange
        var userId = 1;
        var employmentId = 1;

        var employment = new Employment() 
        { 
            Id = employmentId, 
            Company = "Company", 
            Salary = 1000, 
            StartDate = DateTime.Now.AddYears(-1), 
            EndDate = DateTime.Now 
        };

    
        var user = new User() 
        { 
            Id = userId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = null,
            Employments = [employment]
        };

        _userDbServiceMock
            .Setup(x => x.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var actionResult = await _sut.DeleteUserEmployment(userId, employmentId);
        Assert.IsType<NoContentResult>(actionResult);
    }

}