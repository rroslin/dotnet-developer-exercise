using Domain;

namespace Application;

public record GetUserResponse(
    int Id, 
    string FirstName, 
    string LastName, 
    string Email, 
    UserAddress? Address,
    IReadOnlyCollection<UserEmploymentResponse> Employments
);

public static class GetUserMappingExtensions
{
    public static GetUserResponse ToGetUserByIdResponse(this User user)
    {
        return new GetUserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address?.ToUserAddress(),
            user.Employments.Select(e => e.ToUserEmploymentResponse()).ToList().AsReadOnly()
        );
    }
}