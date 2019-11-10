using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AutoMapper.QueryableExtensions;

using Instagraph.Data;
using Instagraph.DTOs.Import;
using Instagraph.Models;

using Newtonsoft.Json;

namespace Instagraph.Import
{
    public static class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";
        private const string PictureSuccessfullyImportedMessage = "Successfully imported Picture {0}.";
        private const string UserSuccessfullyImportedMessage = "Successfully imported User {0}.";
        private const string PostSuccessfullyImportedMessage = "Successfully imported Post {0}.";
        private const string CommentSuccessfullyImportedMessage = "Successfully imported Comment {0}.";
        private const string FollowerSuccessfullyImportedMessage = "Successfully imported Follower {0} to User {1}.";

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, results, true);
        }

        public static string ImportPictures(InstagraphDbContext db, string picturesJson)
        {
            PictureDto[] pictures = JsonConvert.DeserializeObject<PictureDto[]>(picturesJson);

            StringBuilder result = new StringBuilder();

            List<PictureDto> validPictures = new List<PictureDto>();

            foreach (PictureDto picture in pictures)
            {
                bool isValid = IsValid(picture);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validPictures.Add(picture);

                result.AppendLine(string.Format(
                    PictureSuccessfullyImportedMessage,
                    picture.Path));
            }

            Picture[] picturesToAdd = validPictures
                .AsQueryable()
                .ProjectTo<Picture>()
                .ToArray();

            db.Pictures.AddRange(picturesToAdd);

            db.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(InstagraphDbContext db, string usersJson)
        {
            UserDto[] users = JsonConvert.DeserializeObject<UserDto[]>(usersJson);

            StringBuilder result = new StringBuilder();

            List<UserDto> validUsers = new List<UserDto>();

            Dictionary<string, Picture> pictureByPath = users
                .Where(u => u.ProflePicture != null)
                .Select(u => u.ProflePicture)
                .Distinct()
                .ToDictionary(p => p, p => db.Pictures.FirstOrDefault(pic => pic.Path == p));

            foreach (UserDto user in users)
            {
                bool isValid = IsValid(user);

                if (!isValid || pictureByPath[user.ProflePicture] == null)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                validUsers.Add(user);

                result.AppendLine(string.Format(
                    UserSuccessfullyImportedMessage,
                    user.Username));
            }

            User[] usersToAdd = validUsers
                .Select(u => new User
                {
                    Username = u.Username,
                    Password = u.Password,
                    ProfilePicture = pictureByPath[u.ProflePicture]
                })
                .ToArray();

            db.Users.AddRange(usersToAdd);

            db.SaveChanges();

            return result.ToString().TrimEnd();
        }

        //public static string ImportUsersFollowers(InstagraphDbContext db, string usersFollowersJson)
        //{
        //    UserFollowerDto[] usersFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(usersFollowersJson);

        //    StringBuilder result = new StringBuilder();

        //    List<UserFollower> usersFollowersToAdd = new List<UserFollower>();

        //    Dictionary<string, User> userByUsername = usersFollowers
        //        .Select(uf => uf.User)
        //        .Concat(usersFollowers.Select(uf => uf.Follower))
        //        .Distinct()
        //        .ToDictionary(u => u, username => db.Users
        //            .FirstOrDefault(u => u.Username == username));

        //    foreach (UserFollowerDto userFollower in usersFollowers)
        //    {
        //        User user = userByUsername[userFollower.User];
        //        User follower = userByUsername[userFollower.Follower];

        //        if (userFollower.User == userFollower.Follower ||
        //            user == null ||
        //            follower == null ||
        //            usersFollowersToAdd
        //                .Any(uf => uf.UserId == user.Id && 
        //                    uf.FollowerId == follower.Id))
        //        {
        //            result.AppendLine(ErrorMessage);

        //            continue;
        //        }

        //        usersFollowersToAdd.Add(new UserFollower
        //        {
        //            UserId = user.Id,
        //            FollowerId = follower.Id
        //        });

        //        result.AppendLine(string.Format(
        //            FollowerSuccessfullyImportedMessage,
        //            follower.Username,
        //            user.Username));
        //    }

        //    db.UsersFollowers.AddRange(usersFollowersToAdd);

        //    db.SaveChanges();

        //    return result.ToString().TrimEnd();
        //}

        public static string ImportUsersFollowers(InstagraphDbContext db, string usersFollowersJson)
        {
            UserFollowerDto[] usersFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(usersFollowersJson);

            StringBuilder result = new StringBuilder();

            Dictionary<string, User> userByUsername = usersFollowers
                .Select(uf => uf.User)
                .Concat(usersFollowers.Select(uf => uf.Follower))
                .Distinct()
                .ToDictionary(u => u, username => db.Users
                    .FirstOrDefault(u => u.Username == username));

            foreach (UserFollowerDto userFollower in usersFollowers)
            {
                User user = userByUsername[userFollower.User];
                User follower = userByUsername[userFollower.Follower];

                if (userFollower.User == userFollower.Follower ||
                    user == null ||
                    follower == null)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                user.Followers.Add(follower);

                follower.Following.Add(user);

                result.AppendLine(string.Format(
                    FollowerSuccessfullyImportedMessage,
                    follower.Username,
                    user.Username));
            }

            db.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPosts(InstagraphDbContext db, string postsXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("posts");
            XmlSerializer serializer = new XmlSerializer(typeof(PostDto[]), root);

            PostDto[] posts = null;

            using (StringReader reader = new StringReader(postsXml))
            {
                posts = (PostDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            List<Post> postsToAdd = new List<Post>();

            Dictionary<string, User> userByUsername =
                posts.Where(p => p.User != null)
                .Select(p => p.User)
                .Distinct()
                .ToDictionary(u => u, username => db.Users
                    .FirstOrDefault(u => u.Username == username));

            Dictionary<string, Picture> pictureByPath =
                posts.Where(p => p.Picture != null)
                .Select(p => p.Picture)
                .Distinct()
                .ToDictionary(p => p, path => db.Pictures
                    .FirstOrDefault(p => p.Path == path));

            foreach (PostDto post in posts)
            {
                bool isValid = IsValid(post);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                User user = userByUsername[post.User];

                Picture picture = pictureByPath[post.Picture];

                if (user == null || picture == null)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                postsToAdd.Add(new Post
                {
                    Caption = post.Caption,
                    User = user,
                    Picture = picture
                });

                result.AppendLine(string.Format(
                    PostSuccessfullyImportedMessage,
                    post.Caption));
            }

            db.Posts.AddRange(postsToAdd);

            db.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportComments(InstagraphDbContext db, string commentsXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("comments");
            XmlSerializer serializer = new XmlSerializer(typeof(CommentDto[]), root);

            CommentDto[] comments = null;

            using (StringReader reader = new StringReader(commentsXml))
            {
                comments = (CommentDto[]) serializer.Deserialize(reader);
            }

            StringBuilder result = new StringBuilder();

            List<Comment> commentsToAdd = new List<Comment>();

            Dictionary<string, User> userByUsername = comments
                .Where(c => c.User != null)
                .Select(c => c.User)
                .Distinct()
                .ToDictionary(username => username, username => db.Users
                    .FirstOrDefault(u => u.Username == username));

            Dictionary<int, Post> postById = comments
                .Where(c => c.Post != null)
                .Select(c => c.Post.Id)
                .Distinct()
                .ToDictionary(pId => pId, pId => db.Posts
                    .FirstOrDefault(p => p.Id == pId));

            foreach (CommentDto comment in comments)
            {
                bool isValid = IsValid(comment);

                if (!isValid)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                User user = userByUsername[comment.User];

                Post post = postById[comment.Post.Id];

                if (user == null || post == null)
                {
                    result.AppendLine(ErrorMessage);

                    continue;
                }

                commentsToAdd.Add(new Comment
                {
                    Content = comment.Content,
                    Post = post,
                    User = user
                });

                result.AppendLine(string.Format(
                    CommentSuccessfullyImportedMessage,
                    comment.Content));
            }

            db.Comments.AddRange(commentsToAdd);

            db.SaveChanges();

            return result.ToString().TrimEnd();
        }
    }
}