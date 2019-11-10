using PhotoShare.Client.Core.Contracts;
using System;

namespace PhotoShare.Client.Core.Commands
{
    public class LogoutCommand : ICommand
    {
        private const string LogInFirstMessage = "You should log in first in order to logout.";
        private const string SuccessfulLogoutMessage = "User {0} successfully logged out!";

        public int RequiredArgumentsCount => 0;

        public string Execute(string[] args)
        {
            if (!Session.IsUserLoggedIn)
            {
                throw new InvalidOperationException(LogInFirstMessage);
            }

            string result = string.Format(SuccessfulLogoutMessage, Session.User.Username);

            Session.User = null;

            return result;
        }
    }
}