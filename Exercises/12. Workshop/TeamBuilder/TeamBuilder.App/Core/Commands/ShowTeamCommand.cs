using System;
using System.Linq;
using System.Text;

using TeamBuilder.App.Core.DTOs;
using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowTeamCommand : ICommand
    {
        private readonly ITeamService teamService;

        public ShowTeamCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);

            string teamName = arguments[0];

            bool teamExists = this.teamService.Exists(teamName);

            if (!teamExists)
            {
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.TeamNotFound,
                    teamName));
            }
            
            TeamWithMembersDto teamDto = this.teamService.ByName<TeamWithMembersDto>(teamName);

            StringBuilder result = new StringBuilder();

            result.AppendLine($"{teamDto.Name} {teamDto.Acronym}");
            result.AppendLine("Members:");

            result.Append(string.Join(
                Environment.NewLine,
                teamDto.Members.Select(m => $"--{m}")));

            return result.ToString().TrimEnd();
        }
    }
}