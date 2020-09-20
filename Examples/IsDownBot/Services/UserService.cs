using System.Linq;
using IsDownBot.Options;
using Microsoft.Extensions.Options;

namespace IsDownBot.Services
{
    public class UserService : IUserService
    {
        private readonly UsersOptions _usersOptions;

        public UserService(IOptions<UsersOptions> usersOptions)
        {
            _usersOptions = usersOptions.Value;
        }

        public bool IsAdmin(int userId)
        {
            return _usersOptions.AdminIds != null &&
                   _usersOptions.AdminIds.Contains(userId);
        }
    }
}
