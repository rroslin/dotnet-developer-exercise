using System;
using Application.Common;
using Domain;

namespace Application;

public record GetUserRequest(int Id);
public record GetUserResponse(int Id, string FirstName, string LastName, string Email, UserAddress? Address);

public static class GetUserMappingExtensions
{
    public static GetUserResponse ToGetUserByIdResponse(this User user)
    {
        return new GetUserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Address?.ToUserAddress()
        );
    }
}