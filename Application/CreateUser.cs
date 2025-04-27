using Domain;
using FluentValidation;

namespace Application;

public record CreateUserRequest(
    string FirstName, 
    string LastName, 
    string Email, 
    UserAddress? Address, 
    IReadOnlyCollection<UserEmploymentRequest> Employments
);

public record CreateUserResponse(
    int Id, 
    string FirstName, 
    string LastName, 
    string Email, 
    UserAddress? Address, 
    IReadOnlyCollection<UserEmploymentResponse> Employments
);

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.Address)
            .SetValidator(new UserAddressValidator());

        RuleForEach(x => x.Employments)
            .SetValidator(new UserEmploymentRequestValidator())
            .When(x => x.Employments != null && x.Employments.Count > 0, ApplyConditionTo.CurrentValidator);
    }
}
public static class CreateUserMappingExtensions
{
    public static User ToUser(this CreateUserRequest request)
    {
        return new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Address = request.Address?.ToAddress()
        };
    }
    public static CreateUserResponse ToCreateUserResponse(this User user)
    {
        return new CreateUserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address?.ToUserAddress(),
            user.Employments.Select(e => e.ToUserEmploymentResponse()).ToList().AsReadOnly()
        );
    }
}