using System;
using System.Collections.Generic;

namespace TeamBuilder.Models
{
    public class Event
    {
        public Event()
        {
            this.ParticipatingEventTeams = new HashSet<EventTeam>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<EventTeam> ParticipatingEventTeams { get; set; }
    }
}