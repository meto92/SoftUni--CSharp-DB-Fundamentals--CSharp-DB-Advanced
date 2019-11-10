namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models.Enums;
    using Services.Contracts;

    public class CreateAlbumCommand : Command
    {
        private const string UserNotFoundMessage = "User {0} not found!";
        private const string AlbumExistsMessage = "Album {0} exists!";
        private const string ColorNotFoundMessage = "Color {0} not found!";
        private const string InvalidTagsMessage = "Invalid tags!";
        private const string AlbumSuccessfullyCreatedMessage = "Album {0} successfully created!";

        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
        }

        public override int RequiredArgumentsCount => 3;

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public override string Execute(string[] data)
        {
            string username = data[0];
            string albumTitle = data[1];
            string bgColor = data[2];
            string[] tagNames = data.Skip(3)
                .Select(TagUtilities.ValidateOrTransform)
                .ToArray();

            //bool userExist = this.userService.Exists(username);

            //if (!userExist)
            //{
            //    throw new ArgumentException(string.Format(
            //        UserNotFoundMessage,
            //        username));
            //}

            UserUtilities.ValidateLoggedInUserAction(username);

            bool albumExists = this.albumService.Exists(albumTitle);

            if (albumExists)
            {
                throw new ArgumentException(string.Format(
                    AlbumExistsMessage,
                    albumTitle));
            }

            if (!Enum.TryParse(bgColor, out Color color))
            {
                throw new ArgumentException(string.Format(
                    ColorNotFoundMessage,
                    bgColor));
            }

            bool tagsExist = tagNames.All(t => this.tagService.Exists(t));

            if (!tagsExist)
            {
                throw new ArgumentException(InvalidTagsMessage);
            }

            //int userId = this.userService.ByUsername<User>(username).Id;
            int userId = Session.User.Id;

            this.albumService.Create(userId, albumTitle, bgColor, tagNames);

            string result = string.Format(AlbumSuccessfullyCreatedMessage, albumTitle);

            return result;
        }
    }
}