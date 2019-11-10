using System;
using System.Linq;

using PhotoShare.Client.Core.Dtos;
using PhotoShare.Services.Contracts;

namespace PhotoShare.Client.Core.Commands
{
    public class ListFriendsCommand : Command
    {
        private const string UserNotFoundMessage = "User {0} not found!";
        private const string NoFriendsMessage = "No friends for this user. :(";

        private readonly IUserService userService;

        public ListFriendsCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public override int RequiredArgumentsCount => 1;

        public override string Execute(string[] args)
        {
            string username = args[0];

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException(string.Format(
                    UserNotFoundMessage,
                    username));
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);

            if (user.Friends.Count == 0)
            {
                return NoFriendsMessage;
            }

            string friends = string.Join(
                Environment.NewLine,
                user.Friends.Select(f => $"-{f.Username}"));

            string result = $"Friends{Environment.NewLine}" + friends;

            return result;
        }
    }
}