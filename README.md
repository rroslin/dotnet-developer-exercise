# Dotnet Developer Exercise

## Overview
This project is a .NET API developed to demonstrate proficiency building and refining requirements, as well as implementing a RESTful API with a focus on best practices.

## Local Requirements
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Initial Requirements
Develop a simple REST API, meant to allow creation/update and reading of users.

We expect one API with three routes:
- **GET** `api/users/{userId}`: to retrieve a user by ID. 

- **POST** `api/users` to create a user:
    - Ensure the email is unique across the entire DbContext.
    - For employments, `EndDate` should be greater than `StartDate`.

- **PUT** `api/users/{userId}` to update a user, their address, and employment (considered an asset):
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
This are points that I would have raised to be redefined or clarified.

Class Definition:

- `MonthsOfExperience` should not be assigned or modifiable in `Employment`. This can cause issues where `MonthsOfExperience` does not match `StartDate` and `EndDate`. This issue can be avoided by deriving `MonthsOfExperience` from `StartDate` and `EndDate`.

- If `EndDate` is null, it should be assumed that the employment is ongoing. In this case, `MonthsOfExperience` should be computed from `StartDate` to the current date.

- Salary should not use `uint` because currencies are not stored as whole numbers. Instead, `decimal` should be used as it is more suitable for financial and monetary calculations.

- `PostCode` should not use `int`, as some countries use alphanumeric postal codes. To ensure international compatibility, `string` should be used instead.

- Email formatting should be validated to prevent bad data from being entered.

- Mandatory fields should not be nullable unless there is a specific use case for nullability. This should be clarified during the requirements phase.

API Endpoints:

Based on the requirements, Employment is tightly coupled with the PUT endpoint of the User, which introduces unnecessary complexity and violates the principle of separation of concerns. Managing employment data (e.g., adding or deleting records) within the PUT endpoint creates several risks. It complicates maintenance, as decoupling Employment and User later would be a complex and error-prone process if requirements change.

Additionally, it poses risks on the client side, such as misinterpreting operations (e.g., deleting and creating employment records in the same request), making it challenging to track changes to Employment and potentially causing issues with data persistence. A better solution would be to create dedicated nested endpoints for Employment, allowing for decoupling while maintaining cohesion.

## Finalized Requirements

Given all parties approve of the refinement. This is how I would redefine the requirements.

- **GET** `api/users/{userId}`: to retrieve a user by ID. Also returns the employment collection.
    - Response should display accurate `MonthsOfExperience` based from `StartDate` and `EndDate`.
    - If `EndDate` is null, should display accurate `MonthsOfExperience` based from `StartDate` and current date.

- **POST** `api/users`: to create a new user
    - Ensure the email is unique across the entire DbContext.
    - Ensure the email is a valid format.
    - Ensure Employment `EndDate` should be greater than `StartDate`.
    - Response should display accurate `MonthsOfExperience` based from `StartDate` and `EndDate`.
    - If `EndDate` is null, should display accurate `MonthsOfExperience` based from `StartDate` and current date.
    
- **PUT** `api/users/{userId}`: to update an existing user, and their address. (Does not include employments)
    - Ensure the email is unique across the entire DbContext.
    - Ensure the email is a valid format.

- **POST** `api/user/{userId}/employments`: to create new user employment.
    - Ensure `EndDate` should be greater than `StartDate`.
    - Response should display accurate `MonthsOfExperience` based from `StartDate` and `EndDate`.
    - If `EndDate` is null, should display accurate `MonthsOfExperience` based from `StartDate` and current date.

- **DELETE** `api/user/{userId}/employments`: to delete an existing user employment

See Implementation for updated class definitions.

## Things to Improve
Things I would have improved if I had more time:

- Add Integration Tests
- Better organization of the project structure
- Using actual DB for testing
- Use better abstraction to reduce code duplication

## License
This project is licensed under the [MIT License](LICENSE).