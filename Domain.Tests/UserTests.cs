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

    [Fact]
    public void User_AddEmployment_ShouldAddEmployment()
    {
        // Arrange
        var user = new User()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = new Address()
            {
                Street = "123 Main St",
                City = "Anytown",
                PostCode = "12345"
            }
        };

        var employment = new Employment()
        {
            Company = "Tech Company",
            Salary = 60000,
            StartDate = new DateTime(2020, 1, 1),
            EndDate = null
        };

        // Act
        user.AddEmployment(employment);

        // Assert
        Assert.Single(user.Employments);
        Assert.Contains(employment, user.Employments);
    }

    [Fact]
    public void User_RemoveEmployment_ShouldRemoveEmployment()
    {
        // Arrange
        var user = new User()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Address = new Address()
            {
                Street = "123 Main St",
                City = "Anytown",
                PostCode = "12345"
            }
        };
        var employment = new Employment()
        {
            Company = "Tech Company",
            Salary = 60000,
            StartDate = new DateTime(2020, 1, 1),
            EndDate = null
        };
        user.AddEmployment(employment);
        Assert.Single(user.Employments);
        // Act
        user.RemoveEmployment(employment);
        // Assert
        Assert.Empty(user.Employments);
        Assert.DoesNotContain(employment, user.Employments);    
    }
}
