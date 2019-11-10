using PhotoShare.Client.Core;
using System;

namespace PhotoShare.Client.Utilities
{
    public static class UserUtilities
    {
        private const string InvalidCredentialsMessage = "Invalid credentials!";

        public static void ValidateLoggedInUserAction(string username, bool condition = true)
        {
            if (!Session.IsUserLoggedIn ||
                Session.User.Username != username ||
                !condition)
            {
                throw new InvalidOperationException(InvalidCredentialsMessage);
            }
        }

        public static void ValidateUnloggedInUserAction()
        {
            if (Session.IsUserLoggedIn)
            {
                throw new InvalidOperationException(InvalidCredentialsMessage);
            }
        }
    }
}