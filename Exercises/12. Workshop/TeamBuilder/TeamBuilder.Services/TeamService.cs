using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using TeamBuilder.Data;
using TeamBuilder.Models;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.Services
{
    public class TeamService : ITeamService
    {
        private readonly TeamBuilderContext db;

        public TeamService(TeamBuilderContext db)
        {
            this.db = db;
        }

        private IEnumerable<TModel> By<TModel>(Func<Team, bool> predicate) =>
            this.db.Teams
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public TModel ByName<TModel>(string teamName)
            => this.By<TModel>(t => t.Name == teamName).FirstOrDefault();

        public bool Exists(string teamName)
            => this.db.Teams.Any(t => t.Name == teamName);

        public void CreateTeam(string teamName, int creatorId, string acronym, string description)
        {
            Team team = new Team
            {
                Name = teamName,
                CreatorId = creatorId,
                Acronym = acronym,
                Description = description
            };

            this.db.Teams.Add(team);

            this.db.SaveChanges();
        }

        public bool IsUserCreatorOfTeam(string teamName, int userId)
            => this.ByName<Team>(teamName).CreatorId == userId;

        public bool IsUserMemberOfTeam(string teamName, int userId)
            => this.ByName<Team>(teamName).TeamUsers.Any(ut => ut.UserId == userId);

        public void AddMember(int teamId, int userId)
        {
            UserTeam userTeam = new UserTeam
            {
                TeamId = teamId,
                UserId = userId
            };

            this.db.Add(userTeam);

            this.db.SaveChanges();
        }

        public void KickMember(string teamName, int userToBeKickedId)
        {
            Team team = this.ByName<Team>(teamName);

            UserTeam userTeamToDelete = team.TeamUsers.First(ut => ut.UserId == userToBeKickedId);

            team.TeamUsers.Remove(userTeamToDelete);

            this.db.SaveChanges();
        }

        public void Disband(string teamName)
        {
            Team team = this.ByName<Team>(teamName);

            this.db.Entry(db.Teams.Find(team.Id)).State = EntityState.Detached;

            this.db.Teams.Remove(team);

            this.db.SaveChanges();
        }
    }
}