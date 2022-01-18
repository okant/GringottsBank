using System;
using System.ComponentModel.DataAnnotations;

namespace Gringotts.Identity.Service.Dtos
{
    public record UserDto(
        Guid Id,
        string Username,
        string Email,
        DateTimeOffset CreatedDate
    );

    public record UpdateUserDto(
        [Required][EmailAddress] string Email
    );
}