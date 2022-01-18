using Gringotts.Identity.Service.Dtos;
using Gringotts.Identity.Service.Entities;

namespace Gringotts.Identity.Service
{
    public static class Extensions
    {
        public static UserDto AsDto(this ApplicationUser user)
        {
            return new UserDto(
                user.Id,
                user.UserName,
                user.Email,
                user.CreatedOn);
        }
    }
}