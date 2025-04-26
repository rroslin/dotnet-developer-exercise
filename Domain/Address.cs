using System;

namespace Domain;

public class Address
{
    // Mandatory: Street, City
    public int Id { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public string? PostCode { get; set; }
}
