namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models.Enums;
    using PhotoShare.Services.Contracts;

    public class AddTagToCommand : Command
    {
        private const string TagOrAlbumDoesNotFoundMessage = "Either tag or album do not exist!";
        private const string TagAddedMessage = "Tag {0} added to {1}!";

        private readonly IAlbumService albumService;
        private readonly ITagService tagService;
        private readonly IAlbumTagService albumTagService;

        public AddTagToCommand(
            IAlbumService albumService, 
            ITagService tagService, 
            IAlbumTagService albumTagService)
        {
            this.albumService = albumService;
            this.tagService = tagService;
            this.albumTagService = albumTagService;
        }
        
        // AddTagTo <albumName> <tag>
        public override string Execute(string[] args)
        {
            string albumName = args[0];
            string tagName = args[1].ValidateOrTransform();

            bool albumExists = this.albumService.Exists(albumName);
            bool tagExists = this.tagService.Exists(tagName);

            if (!albumExists || !tagExists)
            {
                throw new ArgumentException(TagOrAlbumDoesNotFoundMessage);
            }

            AlbumRolesDto albumRolesDto = this.albumService.ByName<AlbumRolesDto>(albumName);

            bool isLoggedInUserOwner = albumRolesDto.AlbumRoles
                .Any(ar => ar.Role == Role.Owner &&
                    ar.Username == Session.User?.Username);

            UserUtilities.ValidateLoggedInUserAction(Session.User?.Username, isLoggedInUserOwner);

            TagDto tag = this.tagService.ByName<TagDto>(tagName);

            this.albumTagService.AddTagTo(albumRolesDto.Album.Id, tag.Id);

            string result = string.Format(TagAddedMessage, tagName, albumName);

            return result;
        }
    }
}