namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models;
    using PhotoShare.Models.Enums;
    using PhotoShare.Services.Contracts;

    public class ShareAlbumCommand : Command
    {
        private const string AlbumNotFoundMessage = "Album {0} not found!";
        private const string UserNotFoundMessage = "User {0} not found!";
        private const string InvalidPermissionMessage = @"Permission must be either ""Owner"" or ""Viewer""!";
        private const string Successessage = "Username {0} added to album {1} ({2})";

        private IAlbumService albumService;
        private IUserService userService;
        private IAlbumRoleService albumRoleService;
        
        public ShareAlbumCommand(
            IAlbumService albumService, 
            IUserService userService, 
            IAlbumRoleService albumRoleService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.albumRoleService = albumRoleService;
        }

        public override int RequiredArgumentsCount => 3;

        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public override string Execute(string[] data)
        {
            int albumId = int.Parse(data[0]);
            string username = data[1];
            string permission = data[2];

            bool albumExists = this.albumService.Exists(albumId);

            if (!albumExists)
            {
                throw new ArgumentException(string.Format(
                    AlbumNotFoundMessage,
                    albumId));
            }

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException(string.Format(
                    UserNotFoundMessage,
                    username));
            }

            if (!Enum.TryParse(permission, out Role role))
            {
                throw new ArgumentException(InvalidPermissionMessage);
            }

            AlbumRolesDto albumRolesDto = this.albumService.ById<AlbumRolesDto>(albumId);

            bool isLoggedInUserOwner = albumRolesDto.AlbumRoles
                .Any(ar => ar.Role == Role.Owner &&
                    ar.Username == Session.User?.Username);

            UserUtilities.ValidateLoggedInUserAction(Session.User?.Username, isLoggedInUserOwner);

            User user = this.userService.ByUsername<User>(username);

            this.albumRoleService.PublishAlbumRole(albumId, user.Id, permission);

            string result = string.Format(
                Successessage, 
                username, albumRolesDto.Album.Name, permission);

            return result;
        }
    }
}