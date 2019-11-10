namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models.Enums;
    using Services.Contracts;

    public class UploadPictureCommand : Command
    {
        private const string AlbumNotFoundMessage = "Album {0} not found!";
        private const string PictureAddedToAlbumMessage = "Picture {0} added to {1}!";

        private readonly IPictureService pictureService;
        private readonly IAlbumService albumService;

        public UploadPictureCommand(IPictureService pictureService, IAlbumService albumService)
        {
            this.pictureService = pictureService;
            this.albumService = albumService;
        }

        public override int RequiredArgumentsCount => 3;

        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public override string Execute(string[] data)
        {
            string albumName = data[0];
            string pictureTitle = data[1];
            string path = data[2];

            var albumExists = this.albumService.Exists(albumName);

            if (!albumExists)
            {
                throw new ArgumentException(string.Format(
                    AlbumNotFoundMessage,
                    albumName));
            }

            AlbumRolesDto albumRolesDto = this.albumService.ByName<AlbumRolesDto>(albumName);

            bool isLoggedInUserOwner = albumRolesDto.AlbumRoles
                .Any(ar => ar.Role == Role.Owner &&
                    ar.Username == Session.User?.Username);

            UserUtilities.ValidateLoggedInUserAction(Session.User?.Username, isLoggedInUserOwner);

            int albumId = albumRolesDto.Album.Id;
            
            var picture = this.pictureService.Create(albumId, pictureTitle, path);

            string result = string.Format(PictureAddedToAlbumMessage, pictureTitle, albumName);

            return result;
        }
    }
}