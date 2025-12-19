using Domain.ValueObjects;

namespace Application.DTO
{
    public record UserRegistrationDto(
            string Role,
            string FirstName,
            string LastName,
            string Password,
            Email Email
        );

    public record UserDto(
        string Email,
        string Password
    );
}
