using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TeamBuilder.Models.Enums;

namespace TeamBuilder.Models
{
    public class User
    {
        public User()
        {
            this.CreatedEvents = new HashSet<Event>();

            this.ReceivedInvitations = new HashSet<Invitation>();

            this.UserTeams = new HashSet<UserTeam>();

            this.CreatedTeams = new HashSet<Team>();
        }

        public int Id { get; private set; }

        [MinLength(3)]
        public string Username { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Event> CreatedEvents { get; set; }

        public virtual ICollection<Team> CreatedTeams { get; set; }

        public virtual ICollection<Invitation> ReceivedInvitations { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}