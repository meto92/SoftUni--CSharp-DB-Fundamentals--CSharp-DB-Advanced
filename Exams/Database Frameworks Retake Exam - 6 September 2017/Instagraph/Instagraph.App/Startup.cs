using System;
using System.IO;
using System.Xml.Serialization;
using AutoMapper;
using Instagraph.Data;
using Instagraph.DTOs.Import;
using Instagraph.Export;
using Instagraph.Import;

namespace Instagraph.App
{
    public class Startup
    {
        private const string ResourcesDirectory = "Resources";

        private static void Import(InstagraphDbContext db)
        {
            //string picturesJson = File.ReadAllText($@"{ResourcesDirectory}\pictures.json");
            //string importPicturesResult = Deserializer.ImportPictures(db, picturesJson);

            //Console.WriteLine(importPicturesResult);

            //string usersJson = File.ReadAllText($@"{ResourcesDirectory}\users.json");
            //string importUsersResult = Deserializer.ImportUsers(db, usersJson);

            //Console.WriteLine(importUsersResult);

            string usersFollowersJson = File.ReadAllText($@"{ResourcesDirectory}\users_followers.json");
            string importUsersFollowersResult = Deserializer.ImportUsersFollowers (db, usersFollowersJson);

            //Console.WriteLine(importUsersFollowersResult);

            //string postsXml = File.ReadAllText($@"{ResourcesDirectory}\posts.xml");
            //string importPostsResult = Deserializer.ImportPosts(db, postsXml);

            //Console.WriteLine(importPostsResult);

            //string commentsXml = File.ReadAllText($@"{ResourcesDirectory}\comments.xml");
            //string importCommentsResult = Deserializer.ImportComments(db, commentsXml);

            //Console.WriteLine(importCommentsResult);
        }

        private static void Export(InstagraphDbContext db)
        {
            //Serializer.ExportUncommentedPosts(db);

            //Serializer.ExportPopularUsers(db);

            Serializer.ExportCommentsOnPosts(db);
        }

        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<InstagraphProfile>());

            using (InstagraphDbContext db = new InstagraphDbContext())
            {
                //Import(db);
                Export(db);
            }
        }
    }
}