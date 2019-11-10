using System.Linq;

using TeamBuilder.Data;
using TeamBuilder.Models;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly TeamBuilderContext db;

        public InvitationService(TeamBuilderContext db)
        {
            this.db = db;
        }

        public Invitation ByTeamIdAndInvitedUserId(int teamId, int invitedUserId) =>
            this.db.Invitations
                .Where(i => i.IsActive && 
                    i.TeamId == teamId && 
                    i.InvitedUserId == invitedUserId)
                .FirstOrDefault();

        public void CreateInvitation(int teamId, int invitedUserId)
        {
            Invitation invitation = new Invitation
            {
                TeamId = teamId,
                InvitedUserId = invitedUserId
            };

            this.db.Invitations.Add(invitation);

            this.db.SaveChanges();
        }

        public void Deactivate(int id)
        {
            this.db.Invitations.Find(id).IsActive = false;

            this.db.SaveChanges();
        }

        public bool Exists(int teamId, int invitedUserId) =>
            this.db
                .Invitations
                .Any(i => i.IsActive && 
                    i.TeamId == teamId &&
                    i.InvitedUserId == invitedUserId);
    }
}