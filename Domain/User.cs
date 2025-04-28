namespace Domain;

public class User
{
    // Mandatory: Id, FirstName, LastName, Email
    // Unique: Email

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public Address? Address { get; set; }

    public HashSet<Employment> Employments = [];
}
