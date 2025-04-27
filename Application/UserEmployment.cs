using Domain;
using FluentValidation;

namespace Application;

public record UserEmploymentRequest(
    string Company,
    decimal Salary,
    DateTime StartDate,
    DateTime? EndDate
);

public record UserEmploymentResponse(
    int Id,
    string Company,
    decimal Salary,
    int MonthsOfExperience,
    DateTime StartDate,
    DateTime? EndDate
);

public class UserEmploymentRequestValidator : AbstractValidator<UserEmploymentRequest>
{
    public UserEmploymentRequestValidator()
    {
        RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company name is required.");

        RuleFor(x => x.Salary)
            .NotEmpty()
            .WithMessage("Salary is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");

        When(x => x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to start date.");
        });
    }
}

public static class UserEmploymentMappingExtensions
{
    public static Employment ToEmployment(this UserEmploymentRequest request)
    {
        return new Employment
        {
            Company = request.Company,
            Salary = request.Salary,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
    }
 
    public static UserEmploymentResponse ToUserEmploymentResponse(this Employment employment)
    {
        return new UserEmploymentResponse(
            employment.Id,
            employment.Company,
            employment.Salary,
            employment.MonthsOfExperience,
            employment.StartDate,
            employment.EndDate
        );
    }
}
