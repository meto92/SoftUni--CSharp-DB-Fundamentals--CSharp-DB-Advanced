using System;
using System.Collections.Generic;

namespace TeamBuilder.App.Core.DTOs
{
    public class EventWithTeamsDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<string> ParticipatingTeamNames { get; set; }
    }
}