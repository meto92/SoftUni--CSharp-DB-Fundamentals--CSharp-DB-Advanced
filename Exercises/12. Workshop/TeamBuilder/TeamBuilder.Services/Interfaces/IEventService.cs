using System;

namespace TeamBuilder.Services.Interfaces
{
    public interface IEventService
    {
        bool Exists(string eventName);

        TModel ByName<TModel>(string eventName);

        void CreateEvent(string eventName, int creatorId, string description, DateTime startdate, DateTime endDate);

        bool IsTeamInEvent(string eventName, string teamName);

        void AddTeamToEvent(string eventName, int teamId);
    }
}