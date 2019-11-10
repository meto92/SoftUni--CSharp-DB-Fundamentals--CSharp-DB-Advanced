namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Services.Contracts;

    public class AddFriendCommand : Command
    {
        private const string UserNotFoundMessage = "User {0} not found!";
        private const string AlreadyFriendsMessage = "{0} is already a friend to {1}";
        private const string UserHasAlreadySentRequestMessage = "{0} has already sent friendship request to {1}!";
        private const string FriendHasSentRequestMessage = "{0} has friendship request from {1}";
        private const string SuccessMessage = "Friend {0} added to {1}";

        private readonly IUserService userService;

        public AddFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AddFriend <username1> <username2>
        public override string Execute(string[] data)
        {
            string username1 = data[0];
            string username2 = data[1];

            //bool userExists = this.userService.Exists(username1);

            //if (!userExists)
            //{
            //    throw new ArgumentException(string.Format(
            //        UserNotFoundMessage,
            //        username1));
            //}

            UserUtilities.ValidateLoggedInUserAction(username1);

            bool friendExists = this.userService.Exists(username2);

            if (!friendExists)
            {
                throw new ArgumentException(string.Format(
                    UserNotFoundMessage,
                    username2));
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username1);
            UserFriendsDto friend = this.userService.ByUsername<UserFriendsDto>(username2);

            bool hasUserAlreadyAddedFriend = user.Friends.Any(f => f.Username == username2);
            bool hasFriendAlreadyAddedUser = friend.Friends.Any(f => f.Username == username1);

            if (hasUserAlreadyAddedFriend && hasFriendAlreadyAddedUser)
            {
                throw new InvalidOperationException(string.Format(
                    AlreadyFriendsMessage,
                    username2, username1));
            }

            if (hasUserAlreadyAddedFriend)
            {
                throw new InvalidOperationException(string.Format(
                    UserHasAlreadySentRequestMessage,
                    user.Username, friend.Username));
            }

            if (hasFriendAlreadyAddedUser)
            {
                throw new InvalidOperationException(string.Format(
                    FriendHasSentRequestMessage,
                    friend.Username, user.Username));
            }

            this.userService.AddFriend(user.Id, friend.Id);

            string result = string.Format(SuccessMessage, username2, username1);

            return result;
        }
    }
}