using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;

namespace PhotoShare.Client.Core.Commands
{
    public class LoginCommand : Command
    {
        private const string UserAlreadyLoggedInMessage = "You should logout first!";
        private const string InvalidUsernameOrPasswordMessage = "Invalid username or password!";
        private const string SuccessfulLoginMessage = "User {0} successfully logged in!";

        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public override string Execute(string[] args)
        {
            string username = args[0];
            string password = args[1];

            if (Session.IsUserLoggedIn)
            {
                throw new ArgumentException(UserAlreadyLoggedInMessage);
            }

            bool userExists = this.userService.Exists(username);

            User user = null;

            if (userExists)
            {
                user = this.userService.ByUsername<User>(username);
            }

            if (!userExists || user.Password != password)
            {
                throw new ArgumentException(InvalidUsernameOrPasswordMessage);
            }

            Session.User = user;

            string result = string.Format(SuccessfulLoginMessage, username);

            return result;
        }
    }
}