namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Text.RegularExpressions;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    public class ModifyUserCommand : Command
    {
        private const string UserNotFoundMessage = "User {0} not found!";
        private const string PropertyNotSupportedMessage = "Property {0} not supported!";
        private const string NewValueNotValidMessage = "Value {0} not valid.{1}{2}";
        private const string PropertySuccessfullyModifiedMessage = "User {0} {1} is {2}.";

        private IUserService userService;
        private ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        public override int RequiredArgumentsCount => 3;

        private bool TownExists(string name) 
            => this.townService.ByName<Town>(name) != null;

        private void TryChangePassword(int userId, string newValue, ref string detailedMessage)
        {
            if (!Regex.IsMatch(newValue, "[a-z]") || !Regex.IsMatch(newValue, @"\d"))
            {
                detailedMessage = "Invalid Password";
            }
            else
            {
                this.userService.ChangePassword(userId, newValue);
            }
        }

        private void TrySetBornTown(int userId, string newValue, ref string detailedMessage)
        {
            if (!TownExists(newValue))
            {
                detailedMessage = $"Town {newValue} not found!";
            }
            else
            {
                this.userService.SetBornTown(
                    userId,
                    this.townService.ByName<Town>(newValue).Id);
            }
        }

        private void TryChangeCurrentTown(int userId, string newValue, ref string detailedMessage)
        {
            if (!TownExists(newValue))
            {
                detailedMessage = $"Town {newValue} not found!";
            }
            else
            {
                this.userService.SetCurrentTown(
                    userId,
                    this.townService.ByName<Town>(newValue).Id);
            }
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public override string Execute(string[] data)
        {
            string username = data[0];
            string property = data[1];
            string newValue = data[2];

            UserUtilities.ValidateLoggedInUserAction(username);

            //bool userExists = this.userService.Exists(username);

            //if (!userExists)
            //{
            //    throw new ArgumentException(string.Format(
            //        UserNotFoundMessage, username));
            //}

            //UserDto user = this.userService.ByUsername<UserDto>(username);

            User user = Session.User;

            string detailedMessage = string.Empty;

            switch (property)
            {
                case nameof(User.Password):
                    TryChangePassword(user.Id, newValue, ref detailedMessage);
                    break;
                case nameof(User.BornTown):
                    TrySetBornTown(user.Id, newValue, ref detailedMessage);
                    break;
                case nameof(User.CurrentTown):
                    TryChangeCurrentTown(user.Id, newValue, ref detailedMessage);
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        PropertyNotSupportedMessage,
                        property));
            }

            if (!string.IsNullOrEmpty(detailedMessage))
            {
                throw new ArgumentException(string.Format(
                    NewValueNotValidMessage,
                    newValue, Environment.NewLine, detailedMessage));
            }

            string result = string.Format(
                PropertySuccessfullyModifiedMessage,
                username, property, newValue);

            return result;
        }
    }
}