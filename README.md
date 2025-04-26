# Dotnet API Project

## Overview
This project is a .NET API developed to demonstrate proficiency in building scalable and maintainable RESTful services using ASP.NET Core. 

## Requirements
Develop a simple REST API, meant to allow creation/update and reading of users.

We expect one API with three routes:
- **GET** to retrieve a user by ID.

- **POST** to create a user:
  - Validate the following rules:
    - Ensure the email is unique across the entire DbContext.
    - For employments, `EndDate` should be greater than `StartDate`.

- **PUT** to update a user, their address, and employment (considered an asset):
  - Validate the following rules:
    - Ensure the email is unique across the entire DbContext.
    - For employments, `EndDate` should be greater than `StartDate`.

### Given Class Definitions
```csharp
// Mandatory: Company, MonthsOfExperience, Salary, StartDate
public class Employment
{
    public int Id { get; set; }           
    public string? Company { get; set; }
    public uint? MonthsOfExperience { get; set; } 
    public uint? Salary { get; set; } 
    public DateTime? StartDate { get; set; } 
    public DateTime? EndDate { get; set; }
}

// Mandatory: String City
public class Address
{
    public int Id { get; set; }     
    public string? Street { get; set; }      
    public string? City { get; set; }
    public int? PostCode { get; set; }
}

// Mandatory: FirstName, LastName, Email
// Unique: Email
// Able to add employment, and update or delete existing employment
public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public string? Email { get; set; } 
    public Address? Address { get; set; }

    public List<Employment> Employments { get; set; } = []
}
```

## License
This project is licensed under the [MIT License](LICENSE).