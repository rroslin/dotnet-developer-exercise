using Domain;
using FluentValidation;

namespace Application;

public record UserEmployment(
    string Company,
    decimal Salary,
    DateTime StartDate,
    DateTime? EndDate
);

public class UserEmploymentValidator : AbstractValidator<UserEmployment>
{
    public UserEmploymentValidator()
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
    public static UserEmployment ToUserEmployment(this Employment employment)
    {
        return new UserEmployment(
            employment.Company,
            employment.Salary,
            employment.StartDate,
            employment.EndDate
        );
    }
}
