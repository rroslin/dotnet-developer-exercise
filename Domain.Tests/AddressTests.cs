using System;

namespace Domain.Tests;

public class AddressTests
{
    [Fact]
    public void Address_Constructor_ShouldInitializeProperties()
    {
        Address address = new()
        {
            Street = "123 Main St",
            City = "Anytown",
            PostCode = "12345"
        };

        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("Anytown", address.City);
        Assert.Equal("12345", address.PostCode);
    }

    [Fact]
    public void Address_PostCode_ShouldAcceptAlphanumericValues()
    {
        Address address = new()
        {
            Street = "123 Main St",
            City = "Anytown",
            PostCode = "A1B2C3"
        };

        Assert.Equal("A1B2C3", address.PostCode);
    }
}
