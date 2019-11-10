using System;

using TeamBuilder.App.Core.DTOs;
using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core
{
    public static class AuthenticationManager
    {
        private static UserDto currentUser;

        public static bool IsAuthenticated => currentUser != null;

        public static void Authorize()
        {
            if (!IsAuthenticated)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
        }

        public static void Login(UserDto user)
        {
            currentUser = user;
        }

        public static void Logout()
        {
            Authorize();

            currentUser = null;
        }

        public static UserDto GetCurrentUser()
        {
            Authorize();

            return currentUser;
        }
    }
}