using FluentValidation;

namespace Application;

public record CreateUserEmploymentRequest(
    int UserId,
    string CompanyName,
    string JobTitle,
    DateTime StartDate,
    DateTime? EndDate
);

public record CreateUserEmploymentResponse(
    int UserId,
    string CompanyName,
    string JobTitle,
    int MonthsOfExperience,
    DateTime StartDate,
    DateTime? EndDate
);

public class CreateUserEmploymentRequestValidator : AbstractValidator<CreateUserEmploymentRequest>
{
    public CreateUserEmploymentRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("User ID is required.");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required.");

        RuleFor(x => x.JobTitle)
            .NotEmpty()
            .WithMessage("Job title is required.");

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