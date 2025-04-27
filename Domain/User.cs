namespace Domain;

public class User
{
    // Mandatory: Id, FirstName, LastName, Email
    // Unique: Email

    private readonly HashSet<Employment> _employment = [];

    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public Address? Address { get; set; }

    public IReadOnlyCollection<Employment> Employments => _employment.ToList().AsReadOnly();
    
    public void AddEmployment(Employment employment)
    {
        ArgumentNullException.ThrowIfNull(employment);

        _employment.Add(employment);
    }

    public void RemoveEmployment(Employment employment)
    {
        ArgumentNullException.ThrowIfNull(employment);

        _employment.Remove(employment);
    }
}
