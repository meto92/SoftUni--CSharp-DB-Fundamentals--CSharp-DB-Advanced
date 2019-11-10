namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class AcceptFriendCommand : Command
    {
        private const string AlreadyFriendsMessage = "{0} is already a friend to {1}";
        private const string UserNotFoundMessage = "User {0} not found!";
        private const string NoSuchFriendRequestMessage = "{0} has not added {1} as a friend";
        private const string FriendshipAcceptedMessage = "Friend {0} accepted {1} as a friend";

        private IUserService userService;

        public AcceptFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AcceptFriend <username1> <username2>
        public override string Execute(string[] data)
        {
            string username1 = data[0];
            string username2 = data[1];

            if (!this.userService.Exists(username1))
            {
                throw new ArgumentException(string.Format(
                    UserNotFoundMessage,
                    username1));
            }

            if (!this.userService.Exists(username2))
            {
                throw new ArgumentException(string.Format(
                    UserNotFoundMessage,
                    username2));
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username1);
            UserFriendsDto friend = this.userService.ByUsername<UserFriendsDto>(username2);

            bool hasFriendAddedUserAsFriend = friend.Friends.Any(f => f.Username == username1);

            if (user.Friends.Any(f => f.Username == username2) &&
                hasFriendAddedUserAsFriend)
            {
                throw new InvalidOperationException(string.Format(
                    AlreadyFriendsMessage,
                    username2, username1));
            }

            if (!hasFriendAddedUserAsFriend)
            {
                throw new InvalidOperationException(string.Format(
                    NoSuchFriendRequestMessage,
                    username2, username1));
            }

            this.userService.AcceptFriend(user.Id, friend.Id);

            string result = string.Format(FriendshipAcceptedMessage, username1, username2);
            
            return result;
        }
    }
}