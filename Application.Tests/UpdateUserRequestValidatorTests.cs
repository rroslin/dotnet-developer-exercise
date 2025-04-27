using System;

namespace Application.Tests;

public class UpdateUserRequestValidatorTests
{
    private readonly UpdateUserRequestValidator _validator;

    public UpdateUserRequestValidatorTests()
    {
        _validator = new UpdateUserRequestValidator();
    }

    [Fact]
    public void UpdateUserRequestValidator_Validate_NotAllowEmptyFirstName()
    {
        // Arrange
        var request = new UpdateUserRequest(1, string.Empty, "LastName", "Email", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.FirstName));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "First name is required.");
    }

    [Fact]
    public void UpdateUserRequestValidator_Validate_NotAllowEmptyLastName()
    {
        // Arrange
        var request = new UpdateUserRequest(1, "FirstName", string.Empty, "Email", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.LastName));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Last name is required.");
    }

    [Fact]
    public void UpdateUserRequestValidator_Validate_NotAllowInvalidEmail()
    {
        // Arrange
        var request = new UpdateUserRequest(1, "FirstName", "LastName", "InvalidEmail", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Email));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "A valid email address is required.");
    }

    [Fact]
    public void UpdateUserRequestValidator_Validate_AllowValidEmail()
    {
        // Arrange
        var request = new UpdateUserRequest(1, "FirstName", "LastName", "first.last@example.com", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }


}
