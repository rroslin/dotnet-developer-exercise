using Domain;
using Application.Common;

namespace Application;

public record GetUserDetailsRequest(int Id);

public record GetUserDetailsResponse(int Id, string FirstName, string LastName, string Email, UserAddress? Address, IReadOnlyCollection<Employment> Employments);

public static class GetUserDetailsByIdMappingExtensions
{
    public static GetUserDetailsResponse ToGetUserDetailsByIdResponse(this User user)
    {
        return new GetUserDetailsResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address?.ToUserAddress(),
            user.Employments
        );
    }
}
