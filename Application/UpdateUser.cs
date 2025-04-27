using Application.Common;
using FluentValidation;

namespace Application;

public record UpdateUserRequest(int Id, string FirstName, string LastName, string Email, UserAddress? Address);
public record UpdateUserResponse(string FirstName, string LastName, string Email, UserAddress? Address);

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Address).SetValidator(new UserAddressValidator());
    }
}

public static class UpdateUserMappingExtensions
{
    public static UpdateUserResponse ToUpdateUserResponse(this Domain.User user)
    {
        return new UpdateUserResponse(
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address?.ToUserAddress()
        );
    }
}
