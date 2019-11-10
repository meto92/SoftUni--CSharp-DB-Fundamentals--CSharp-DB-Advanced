using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamBuilder.Models
{
    public class Team
    {
        public Team()
        {
            this.Invitations = new HashSet<Invitation>();

            this.TeamEvents = new HashSet<EventTeam>();

            this.TeamUsers = new HashSet<UserTeam>();
        }

        public int Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Acronym { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<EventTeam> TeamEvents { get; set; }

        public virtual ICollection<UserTeam> TeamUsers { get; set; }
    }
}