namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Services.Contracts;

    public class RegisterUserCommand : Command
    {
        private const string PasswordsMismatchMessage = "Passwords do not match!";
        private const string UsernameTakenMessage = "Username {0} is already taken!";
        private const string InvalidDataMessage = "Invalid password or email!";
        private const string SuccessfulRegistrationMessage = "User {0} was registered successfully!";

        private IUserService userService;

        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public override int RequiredArgumentsCount => 4;

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public override string Execute(string[] data)
        {
            string username = data[0];
            string password = data[1];
            string repeatedPassword = data[2];
            string email = data[3];

            UserUtilities.ValidateUnloggedInUserAction();

            bool isUsernameAlreadyTaken = this.userService.Exists(username);

            if (isUsernameAlreadyTaken)
            {
                throw new InvalidOperationException(string.Format(
                    UsernameTakenMessage,
                    username));
            }

            if (password != repeatedPassword)
            {
                throw new ArgumentException(PasswordsMismatchMessage);
            }

            RegisterUserDto userDto = new RegisterUserDto
            {
                Username = username,
                Password = password,
                Email = email
            };

            if (!IsValid(userDto))
            {
                //throw new ArgumentException(InvalidDataMessage);
            }

            this.userService.Register(username, password, email);

            string result = string.Format(SuccessfulRegistrationMessage, username);

            return result;
        }
    }
}