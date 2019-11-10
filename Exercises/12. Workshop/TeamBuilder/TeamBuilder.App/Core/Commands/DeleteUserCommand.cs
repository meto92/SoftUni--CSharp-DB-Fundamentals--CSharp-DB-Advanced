using TeamBuilder.App.Core.DTOs;
using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class DeleteUserCommand : ICommand
    {
        private readonly IUserService userService;

        public DeleteUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(0, arguments);

            UserDto user = AuthenticationManager.GetCurrentUser();

            this.userService.Delete(user.Id);

            string result = string.Format(
                Constants.SuccessMessages.UserDeleted, 
                user.Username);

            AuthenticationManager.Logout();

            return result;
        }
    }
}