using System;

namespace Domain.Tests;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        Address address = new()
        {
            Street = "123 Main St",
            City = "Anytown",
            PostCode = "12345"
        };
        Employment employment = new()
        {
            Company = "Tech Company",
            Salary = 60000,
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2022, 1, 1)
        };
        var user = new User()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = address
        };

        // Act & Assert
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("john.doe@example.com", user.Email);
        Assert.Equal(address, user.Address);
        Assert.Empty(user.Employments);
    }
}
