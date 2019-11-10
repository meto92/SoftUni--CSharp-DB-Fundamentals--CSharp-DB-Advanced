namespace PhotoShare.Client.Core.Commands
{
    using System;

    using PhotoShare.Client.Utilities;
    using PhotoShare.Models;
    using Services.Contracts;

    public class DeleteUserCommand : Command
    {
        private const string UserAlreadyDeletedMessage = "User {0} is already deleted!";
        private const string UserSuccessfullyDeletedMessage = "User {0} was deleted from the database!";

        private readonly IUserService userService;

        public DeleteUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public override int RequiredArgumentsCount => 1;

        // DeleteUser <username>
        public override string Execute(string[] data)
        {
            string username = data[0];

            //bool userExists = this.userService.Exists(username);

            //if (!userExists)
            //{
            //    throw new ArgumentException($"User {username} not found!");
            //}

            UserUtilities.ValidateLoggedInUserAction(username);

            //var user = this.userService.ByUsername<UserDto>(username);
            User user = Session.User;
            
            if (user.IsDeleted != null && (bool) user.IsDeleted)
            {
                throw new InvalidOperationException(string.Format(
                    UserAlreadyDeletedMessage,
                    username));
            }

            this.userService.Delete(username);
            
            string result = string.Format(UserSuccessfullyDeletedMessage, username);

            return result;
        }
    }
}