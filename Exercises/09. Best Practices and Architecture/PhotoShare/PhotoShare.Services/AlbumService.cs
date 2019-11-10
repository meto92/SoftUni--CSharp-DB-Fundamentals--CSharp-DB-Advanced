using AutoMapper.QueryableExtensions;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Models.Enums;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly PhotoShareContext context;
        private readonly ITagService tagService;

        public AlbumService(PhotoShareContext context, ITagService tagService)
        {
            this.context = context;
            this.tagService = tagService;
        }

        public TModel ById<TModel>(int id) 
            => By<TModel>(a => a.Id == id).SingleOrDefault();

        public TModel ByName<TModel>(string name) 
            => By<TModel>(a => a.Name == name).SingleOrDefault();

        public Album Create(int userId, string albumTitle, string bgColor, string[] tags)
        {
            HashSet<AlbumTag> albumTags = new HashSet<AlbumTag>();

            tags.Select(t => new AlbumTag() { TagId = this.tagService.ByName<Tag>(t).Id })
                .ToList()
                .ForEach(t => albumTags.Add(t));

            Album album = new Album()
            {
                Name = albumTitle,
                BackgroundColor = Enum.Parse<Color>(bgColor),
                AlbumTags = albumTags,
                AlbumRoles = new HashSet<AlbumRole>()
                {
                    new AlbumRole()
                    {
                        UserId = userId,
                        Role = Role.Owner
                    }
                }
            };

            this.context.Albums.Add(album);

            this.context.SaveChanges();

            return album;
        }

        public bool Exists(int id) => ById<Album>(id) != null;

        public bool Exists(string name) => ByName<Album>(name) != null;

        private IEnumerable<TModel> By<TModel>(Func<Album, bool> predicate) =>
            this.context.Albums
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();
    }
}