using ATON_Test_Ploblem.Models;
using ATON_Test_Ploblem.Models.DTO;

namespace ATON_Test_Ploblem.Helpers
{
    static class MapToGetUserDTO
    {
        public static GetUserDTO Map(User user)
        {
            return new GetUserDTO()
            {
                Guid = user.Guid,
                Login = user.Login,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
                RevokedOn = user.RevokedOn,
                RevokedBy = user.RevokedBy,
            };
        }

        public static List<GetUserDTO> Map(List<User> users)
        {
            return users.Select(u => Map(u)).ToList();
        }

    }
}
