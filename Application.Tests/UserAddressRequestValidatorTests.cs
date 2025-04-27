using System;

namespace Application.Tests;

public class UserAddressValidatorTests
{
    private readonly UserAddressValidator _validator;

    public UserAddressValidatorTests()
    {
        _validator = new UserAddressValidator();
    }

    [Fact]
    public void UserAddressValidator_Validate_NotAllowEmptyStreet()
    {
        // Arrange
        var request = new UserAddress(string.Empty,"City",null);


        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.Street));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Street is required.");
    }

    [Fact]
    public void UserAddressValidator_Validate_NotAllowEmptyCity()
    {
        // Arrange
        var request = new UserAddress("Street", string.Empty, null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(request.City));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "City is required.");
    }
}
