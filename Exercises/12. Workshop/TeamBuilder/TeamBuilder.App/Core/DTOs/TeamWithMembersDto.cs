using System.Collections.Generic;

namespace TeamBuilder.App.Core.DTOs
{
    public class TeamWithMembersDto
    {
        public string Name { get; set; }

        public string Acronym { get; set; }

        public ICollection<string> Members { get; set; }
    }
}