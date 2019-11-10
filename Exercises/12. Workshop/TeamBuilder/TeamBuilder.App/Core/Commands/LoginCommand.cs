using System;

using TeamBuilder.App.Core.DTOs;
using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string username = arguments[0];
            string password = arguments[1];

            if (AuthenticationManager.IsAuthenticated)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            UserDto user = this.userService.GetUserByCredentials<UserDto>(username, password);

            if (user == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            AuthenticationManager.Login(user);

            string result = string.Format(Constants.SuccessMessages.Login, username);

            return result;
        }
    }
}