namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Utilities;
    using PhotoShare.Services.Contracts;
    using System;

    public class AddTagCommand : Command
    {
        private const string TagExistsMessage = "Tag {0} exists!";
        private const string TagSuccessfullyAddedMessage = "Tag {0} was added successfully!";

        private readonly ITagService tagService;

        public AddTagCommand(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public override int RequiredArgumentsCount => 1;

        public override string Execute(string[] args)
        {
            string tagName = args[0];

            tagName = tagName.ValidateOrTransform();

            UserUtilities.ValidateLoggedInUserAction(Session.User?.Username);

            bool tagExists = this.tagService.Exists(tagName);

            if (tagExists)
            {
                throw new ArgumentException(string.Format(
                    TagExistsMessage,
                    tagName));
            }

            this.tagService.AddTag(tagName);

            string result = string.Format(TagSuccessfullyAddedMessage, tagName);

            return result;
        }
    }
}