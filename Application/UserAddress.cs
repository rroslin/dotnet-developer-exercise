using Domain;
using FluentValidation;

namespace Application;

public record UserAddress(string Street, string City, string? PostCode);

public class UserAddressValidator : AbstractValidator<UserAddress?>
{
    public UserAddressValidator()
    {
        When(x => x != null, () =>
        {
            RuleFor(x => x!.Street)
                .NotEmpty()
                .WithMessage("Street is required.");

            RuleFor(x => x!.City)
                .NotEmpty()
                .WithMessage("City is required.");
        });

    }
}

public static class UserAddressMappingExtensions
{
    public static UserAddress ToUserAddress(this Address address)
    {
        return new UserAddress(
            address.Street,
            address.City,
            address.PostCode
        );
    }

    public static Address ToAddress(this UserAddress userAddress)
    {
        return new Address
        {
            Street = userAddress.Street,
            City = userAddress.City,
            PostCode = userAddress.PostCode
        };
    }
}