using System;

namespace Domain;

public class Employment
{
    // Mandatory: Id, Company, MonthsOfExperience, Salary, StartDate
    public int Id { get; set; }
    public required string Company { get; set; }
    public required decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int MonthsOfExperience
    {
        get
        {
            if (EndDate != null)
            {
                return (int)((EndDate.Value - StartDate).TotalDays / 30);
            }
            else
            {
                return (int)((DateTime.Now - StartDate).TotalDays / 30);
            }
        }
    }
}
