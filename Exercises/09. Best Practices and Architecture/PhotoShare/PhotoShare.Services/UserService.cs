using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class UserService : IUserService
    {
        private readonly PhotoShareContext context;

        public UserService(PhotoShareContext context)
        {
            this.context = context;
        }

        private Friendship CreateFriendShip(int userId, int friendId)
        {
            Friendship friendship = new Friendship()
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);

            this.context.SaveChanges();

            return friendship;
        }

        public Friendship AcceptFriend(int userId, int friendId)
        {
            Friendship friendship = CreateFriendShip(userId, friendId);

            return friendship;
        }

        public Friendship AddFriend(int userId, int friendId)
        {
            Friendship friendship = CreateFriendShip(userId, friendId);

            return friendship;
        }

        public TModel ById<TModel>(int id) 
            => By<TModel>(u => u.Id == id).SingleOrDefault();

        public TModel ByUsername<TModel>(string username) 
            => By<TModel>(u => u.Username == username).SingleOrDefault();

        public void ChangePassword(int userId, string password) 
            => ById<User>(userId).Password = password;

        public void Delete(string username)
        {
            User user = ByUsername<User>(username);

            user.IsDeleted = true;

            if (context.Entry(user).State == EntityState.Detached)
            {
                context.Entry(user).State = EntityState.Modified; 
            }

            this.context.SaveChanges();
        }

        public bool Exists(int id) 
            => By<User>(u => u.Id == id).SingleOrDefault() != null;

        public bool Exists(string name) 
            => By<User>(u => u.Username == name).SingleOrDefault() != null;

        private IEnumerable<TModel> By<TModel>(Func<User, bool> predicate) =>
            this.context.Users
                .Where(u => //(u.IsDeleted == null || !((bool) u.IsDeleted)) && 
                    predicate(u))
                .AsQueryable()
                .ProjectTo<TModel>();

        public User Register(string username, string password, string email)
        {
            User user = new User()
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false
            };

            this.context.Users.Add(user);

            this.context.SaveChanges();

            return user;
        }

        public void SetBornTown(int userId, int townId)
        {
            ById<User>(userId).BornTownId = townId;

            this.context.SaveChanges();
        }

        public void SetCurrentTown(int userId, int townId)
        {
            ById<User>(userId).CurrentTownId = townId;

            this.context.SaveChanges();
        }
    }
}