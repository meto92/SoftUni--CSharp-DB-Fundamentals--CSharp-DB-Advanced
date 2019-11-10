using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper.QueryableExtensions;

using TeamBuilder.Data;
using TeamBuilder.Models;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.Services
{
    public class EventService : IEventService
    {
        private TeamBuilderContext db;

        public EventService(TeamBuilderContext db)
        {
            this.db = db;
        }

        private IEnumerable<TModel> By<TModel>(Func<Event, bool> predicate) =>
            this.db.Events
                .Where(predicate)
                .OrderByDescending(e => e.StartDate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public void CreateEvent(string eventName, int creatorId, string description, DateTime startdate, DateTime endDate)
        {
            Event newEvent = new Event
            {
                Name = eventName,
                CreatorId = creatorId,
                Description = description,
                StartDate = startdate,
                EndDate = endDate
            };

            this.db.Events.Add(newEvent);

            this.db.SaveChanges();
        }

        public TModel ByName<TModel>(string eventName)
            => this.By<TModel>(e => e.Name == eventName).FirstOrDefault();

        public bool Exists(string eventName)
            => this.ByName<Event>(eventName) != null;

        public bool IsTeamInEvent(string eventName, string teamName)
        {
            Event ev = this.ByName<Event>(eventName);

            bool result = ev.ParticipatingEventTeams.Any(et => et.Team.Name == teamName);

            return result;
        }

        public void AddTeamToEvent(string eventName, int teamId)
        {
            Event ev = this.ByName<Event>(eventName);

            ev.ParticipatingEventTeams.Add(new EventTeam { TeamId = teamId });

            this.db.SaveChanges();
        }
    }
}