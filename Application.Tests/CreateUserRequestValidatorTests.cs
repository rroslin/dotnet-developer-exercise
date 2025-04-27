using System;

namespace Application.Tests;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator;

    public CreateUserRequestValidatorTests()
    {
        _validator = new CreateUserRequestValidator();
    }

    [Fact]
    public void CreateUserRequestValidator_Validate_NotAllowEmptyFirstName()
    {
        // Arrange
        var request = new CreateUserRequest(string.Empty, "LastName", "first.last@example.com", null, []);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.FirstName));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "First name is required.");
    }

    [Fact]
    public void CreateUserRequestValidator_Validate_NotAllowEmptyLastName()
    {
        // Arrange
        var request = new CreateUserRequest("FirstName", string.Empty, "first.last@example.com", null, []);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.LastName));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Last name is required.");
    }

    [Fact]
    public void CreateUserRequestValidator_Validate_NotAllowInvalidEmail()
    {
        // Arrange
        var request = new CreateUserRequest("FirstName", "LastName", "InvalidEmail", null, []);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Email));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "A valid email address is required.");
    }

    [Fact]
    public void CreateUserRequestValidator_Validate_AllowValidEmail()
    {
        // Arrange
        var request = new CreateUserRequest("FirstName", "LastName", "first.last@example.com", null, []);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void CreateUserRequestValidator_Validate_NotAllowInvalidAddress()
    {
        // Arrange
        var address = new UserAddress(string.Empty, string.Empty, null);
        var request = new CreateUserRequest("FirstName", "LastName", "first.last@example.com", address, []);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Street is required.");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "City is required.");
    }

    [Fact]
    public void CreateUserRequestValidator_Validate_AllowValidAddress()
    {
        // Arrange
        var address = new UserAddress("Street", "City", null);
        var addressWithPostalCode = new UserAddress("Street", "City", "12345");
        var request = new CreateUserRequest("FirstName", "LastName", "first.last@example.com", address, []);
        var requestWithPostalCode = new CreateUserRequest("FirstName", "LastName", "first.last@example.com", addressWithPostalCode, []);

        // Act
        var result = _validator.Validate(request);
        var resultWithPostalCode = _validator.Validate(requestWithPostalCode);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.True(resultWithPostalCode.IsValid);
        Assert.Empty(resultWithPostalCode.Errors);
    }
}
