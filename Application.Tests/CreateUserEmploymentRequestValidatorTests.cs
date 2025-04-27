using System;

namespace Application.Tests;

public class CreateUserEmploymentRequestValidatorTests
{
    private readonly CreateUserEmploymentRequestValidator _validator;

    public CreateUserEmploymentRequestValidatorTests()
    {
        _validator = new CreateUserEmploymentRequestValidator();
    }

    [Fact]
    public void CreateUserEmploymentRequestValidator_Validate_NotAllowEmptyCompany()
    {
        // Arrange
        var request = new CreateUserEmploymentRequest(1, string.Empty, 50000, DateTime.Now, null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Company));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Company name is required.");
    }

    [Fact]
    public void CreateUserEmploymentRequestValidator_Validate_NotAllowNegativeSalary()
    {
        // Arrange
        var request = new CreateUserEmploymentRequest(1, "Company", -50000, DateTime.Now, null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Salary));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Salary must be greater than or equal to 0.");
    }

    [Fact]
    public void CreateUserEmploymentRequestValidator_Validate_AllowValidSalary()
    {
        // Arrange
        var request = new CreateUserEmploymentRequest(1, "Company", 8300, DateTime.Now, null);
        var requestWithZeroSalary = new CreateUserEmploymentRequest(1, "Company", 0, DateTime.Now, null);
        var requestWithDecimalSalary = new CreateUserEmploymentRequest(1, "Company", 8300.75m, DateTime.Now, null);
        // Act
        var result = _validator.Validate(request);
        var resultWithZeroSalary = _validator.Validate(requestWithZeroSalary);
        var resultWithDecimalSalary = _validator.Validate(requestWithDecimalSalary);

        // Assert
        Assert.True(result.IsValid);
        Assert.True(resultWithZeroSalary.IsValid);
        Assert.True(resultWithDecimalSalary.IsValid);
    }

    [Fact]
    public void CreateUserEmploymentRequestValidator_Validate_NotAllowEndDateBeforeStartDate()
    {
        // Arrange
        var request = new CreateUserEmploymentRequest(1, "Company", 50000, DateTime.Now.AddDays(1), DateTime.Now);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.EndDate));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "End date must be greater than or equal to start date.");
    }

    [Fact]
    public void CreateUserEmploymentRequestValidator_Validate_AllowEmptyEndDate()
    {
        // Arrange
        var request = new CreateUserEmploymentRequest(1, "Company", 50000, DateTime.Now, null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}
