using System;
using System.Linq;

using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models.Enums;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService userService;

        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(7, arguments);
            
            string username = arguments[0];
            string password = arguments[1];
            string repeatPassword = arguments[2];
            string firstName = arguments[3];
            string lastName = arguments[4];
            string ageStr = arguments[5];
            string genderStr = arguments[6];

            if (username.Length < Constants.MinUsernameLength ||
                username.Length > Constants.MaxUsernameLength)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.UsernameNotValid,
                    username));
            }

            if (password.Length < Constants.MinPasswordLength ||
                password.Length > Constants.MaxPasswordLength ||
                !password.Any(c => char.IsDigit(c)) ||
                !password.Any(c => char.IsUpper(c)))
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.PasswordNotValid,
                    password));
            }

            if (!int.TryParse(ageStr, out int age) ||
                age < 0)
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            if (!Enum.TryParse(genderStr, out Gender gender))
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            if (password != repeatPassword)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }

            bool isUsernameTaken = this.userService.Exists(username);

            if (isUsernameTaken)
            {
                throw new InvalidOperationException(string.Format(
                    Constants.ErrorMessages.UsernameIsTaken,
                    username));
            }

            if (AuthenticationManager.IsAuthenticated)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            this.userService.Register(username, password, firstName, lastName, age, gender);

            string result = string.Format(
                Constants.SuccessMessages.UserRegistered, 
                username);

            return result;
        }
    }
}