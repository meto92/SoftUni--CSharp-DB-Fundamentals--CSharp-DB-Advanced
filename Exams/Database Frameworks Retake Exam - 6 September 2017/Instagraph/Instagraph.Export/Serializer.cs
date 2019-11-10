using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Instagraph.Data;
using Instagraph.DTOs.Export;

using Newtonsoft.Json;

namespace Instagraph.Export
{
    public static class Serializer
    {
        private const string ExportDirectory = "ExportedData";

        private static void EnsureExportDirectory()
        {
            bool directoryExists = Directory.Exists(ExportDirectory);

            if (!directoryExists)
            {
                Directory.CreateDirectory(ExportDirectory);
            }
        }

        public static void ExportUncommentedPosts(InstagraphDbContext db)
        {
            PostDto[] uncommentedPosts = db.Posts
                .Where(p => p.Comments.Count() == 0)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    User = p.User.Username,
                    Picture = p.Picture.Path
                })
                .ToArray();

            EnsureExportDirectory();

            string path = $@"{ExportDirectory}\uncommented-posts.json";

            string json = JsonConvert.SerializeObject(uncommentedPosts, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(path, json);
        }

        public static void ExportPopularUsers(InstagraphDbContext db)
        {
            //PopularUserDto[] popularUsers = db.Users
            //    .Where(u => u.Followers
            //        .Select(uf => uf.FollowerId)
            //        .Any(followerId => u.Posts
            //            .SelectMany(p => p.Comments
            //                .Select(c => c.UserId))
            //            .Contains(followerId)))
            //    .OrderByDescending(u => u.Id)
            //    .Select(u => new PopularUserDto
            //    {
            //        Username = u.Username,
            //        FollowersCount = u.Followers.Count()
            //    })
            //    .ToArray();

            PopularUserDto[] popularUsers = db.Users
                .Where(u => u.Posts
                    .SelectMany(p => p.Comments.Select(c => c.UserId))
                    .Any(commenterId => u.Followers
                        .Select(f => f.Id).Contains(commenterId)))
                .OrderByDescending(u => u.Id)
                .Select(u => new PopularUserDto
                {
                    Username = u.Username,
                    FollowersCount = db.UsersFollowers.Count(uf => uf.UserId == u.Id)
                })
                .ToArray();

            EnsureExportDirectory();

            string path = $@"{ExportDirectory}\popular-users.json";

            string json = JsonConvert.SerializeObject(popularUsers, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(path, json);
        }

        public static void ExportCommentsOnPosts(InstagraphDbContext db)
        {
            Dto[] dtos = db.Users
                .Select(u => new Dto
                {
                    Username = u.Username,
                    MostComments = u.Posts.Count == 0
                        ? 0
                        : u.Posts.Select(p => p.Comments.Count()).Max()
                })
                .OrderByDescending(u => u.MostComments)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("users");
            XmlSerializer serializer = new XmlSerializer(typeof(Dto[]), root);
            XmlSerializerNamespaces namespaces =
                new XmlSerializerNamespaces(new[]
                {
                    new XmlQualifiedName(string.Empty, string.Empty)
                });

            EnsureExportDirectory();

            string path = $@"{ExportDirectory}\comments-on-posts.xml";

            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, dtos, namespaces);
            }
        }
    }
}