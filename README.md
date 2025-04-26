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

- `MonthsOfExperience` should not be assigned or modifiable to `Employment`.
This can cause issues down the line where the MonthsOfExperience does not match `StartDate` and `EndDate`. This can be easily avoided by deriving `MonthsOfExperience` from `StartDate` and `EndDate`.

- Assuming `EndDate` is null means the employment is on-going, `MonthsOfExperience` should be computed from `StartDate` and present date.

- Salary shouldn't be `uint` because currencies are not stored as whole numbers, `decimal` should be used as it covers most financial and monetary calculations.

- `PostCode` shouldn't be `int`, some countries use alpha-numeric postal-codes so assuming this api would be used internationally we should use `string` to be safe.

- We should validate `Email` formatting to safeguard from bad data.

- Mandatory fields should not be nullable, unless there is usecase for nullability. (Should be raised for clarification)

## License
This project is licensed under the [MIT License](LICENSE).