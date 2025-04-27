using System;
using Domain;

namespace Domain.Tests;

public class EmploymentTests
{
    [Fact]
    public void Employment_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var employment = new Employment()
        {
            Company = "Tech Company",
            Salary = 60000,
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2022, 1, 1)
        };

        // Act & Assert
        Assert.Equal("Tech Company", employment.Company);
        Assert.Equal(60000, employment.Salary);
        Assert.Equal(new DateTime(2020, 1, 1), employment.StartDate);
        Assert.Equal(new DateTime(2022, 1, 1), employment.EndDate);
    }

    [Fact]
    public void Employment_MonthsOfExperience_ShouldCalculateExperience_WhenEndDateIsNull()
    {
        // Arrange
        var employment = new Employment()
        {
            Company = "Tech Company",
            Salary = 60000,
            StartDate = new DateTime(2020, 1, 1),
            EndDate = null
        };
        var numberOfMonths = (int)((DateTime.Now - employment.StartDate).TotalDays / 30);

        // Act & Assert
        Assert.Equal(new DateTime(2020, 1, 1), employment.StartDate);
        Assert.Null(employment.EndDate);
        Assert.Equal(numberOfMonths, employment.MonthsOfExperience);
    }

    [Fact]
    public void Employment_MonthsOfExperience_ShouldCalculateExperience_WhenEndDateIsNotNull()
    {
        // Arrange
        var employment = new Employment()
        {
            Company = "Tech Company",
            Salary = 60000,
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2022, 1, 1)
        };
        var numberOfMonths = (int)((employment.EndDate.Value - employment.StartDate).TotalDays / 30);

        // Act & Assert
        Assert.Equal(new DateTime(2020, 1, 1), employment.StartDate);
        Assert.Equal(new DateTime(2022, 1, 1), employment.EndDate);
        Assert.NotNull(employment.EndDate);
        Assert.Equal(numberOfMonths, employment.MonthsOfExperience);
    }
}
