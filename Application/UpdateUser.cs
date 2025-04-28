using Domain;
using FluentValidation;

namespace Application;

public record UpdateUserRequest(string FirstName, string LastName, string Email, UserAddress? Address);

public record UpdateUserResponse(
    string FirstName, 
    string LastName, 
    string Email, 
    UserAddress? Address
);

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email address is required.");
        RuleFor(x => x.Address).SetValidator(new UserAddressValidator());
    }
}

public static class UpdateUserMappingExtensions
{
    public static void UpdateUser(this User user, UpdateUserRequest request)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Address = request.Address?.ToAddress();
    }
    
    public static UpdateUserResponse ToUpdateUserResponse(this User user)
    {
        return new UpdateUserResponse(
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address?.ToUserAddress()
        );
    }
}
