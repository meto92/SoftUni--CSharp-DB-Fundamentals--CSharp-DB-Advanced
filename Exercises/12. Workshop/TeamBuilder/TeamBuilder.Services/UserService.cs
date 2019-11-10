using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using TeamBuilder.Data;
using TeamBuilder.Models;
using TeamBuilder.Models.Enums;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.Services
{
    public class UserService : IUserService
    {
        private readonly TeamBuilderContext db;
        private readonly ITeamService teamService;

        public UserService(TeamBuilderContext db, ITeamService teamService)
        {
            this.db = db;
            this.teamService = teamService;
        }

        private IEnumerable<TModel> By<TModel>(Predicate<User> predicate) =>
            this.db.Users
                .Where(u => !u.IsDeleted && predicate(u))
                .AsQueryable()
                .ProjectTo<TModel>();

        public TModel ById<TModel>(int id)
            => this.By<TModel>(u => u.Id == id).FirstOrDefault();

        public TModel ByUsername<TModel>(string username)
            => this.By<TModel>(u => u.Username == username).FirstOrDefault();

        public bool Exists(string username)
            => this.ByUsername<User>(username) != null;

        public void Register(string username, string password, string firstName, string lastName, int age, Gender gender)
        {
            User user = new User
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Gender = gender
            };

            this.db.Users.Add(user);

            this.db.SaveChanges();
        }

        public TModel GetUserByCredentials<TModel>(string username, string password)
            => this.By<TModel>(u => u.Username == username && u.Password == password).FirstOrDefault();

        public void Delete(int id)
        {
            User user = this.ById<User>(id);

            this.db.Entry(this.db.Users.Find(id)).State = EntityState.Detached;

            user.IsDeleted = true;

            this.db.Entry(user).State = EntityState.Modified;
            
            this.db.SaveChanges();
        }
    }
}