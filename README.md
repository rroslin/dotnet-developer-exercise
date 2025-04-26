# Dotnet API Project

## Overview
This project is a .NET API developed to demonstrate proficiency in building scalable and maintainable RESTful services using ASP.NET Core. 

## Requirements
Develop a simple REST API, meant to allow creation/update and reading of users.

We expect one API with three routes:
- **GET** to retrieve a user by ID.

- **POST** to create a user:
    - Ensure the email is unique across the entire DbContext.
    - For employments, `EndDate` should be greater than `StartDate`.

- **PUT** to update a user, their address, and employment (considered an asset):
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

## Refinement
This are points that I would have raised to be redefined or clarified:

- `MonthsOfExperience` should not be assigned or modifiable in `Employment`. This can cause issues where `MonthsOfExperience` does not match `StartDate` and `EndDate`. This issue can be avoided by deriving `MonthsOfExperience` from `StartDate` and `EndDate`.

- If `EndDate` is null, it should be assumed that the employment is ongoing. In this case, `MonthsOfExperience` should be computed from `StartDate` to the current date.

- Salary should not use `uint` because currencies are not stored as whole numbers. Instead, `decimal` should be used as it is more suitable for financial and monetary calculations.

- `PostCode` should not use `int`, as some countries use alphanumeric postal codes. To ensure international compatibility, `string` should be used instead.

- Email formatting should be validated to prevent bad data from being entered.

- Mandatory fields should not be nullable unless there is a specific use case for nullability. This should be clarified during the requirements phase.

## License
This project is licensed under the [MIT License](LICENSE).