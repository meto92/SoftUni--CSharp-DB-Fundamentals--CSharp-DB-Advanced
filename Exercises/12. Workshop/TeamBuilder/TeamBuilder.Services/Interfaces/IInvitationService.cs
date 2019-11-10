using TeamBuilder.Models;

namespace TeamBuilder.Services.Interfaces
{
    public interface IInvitationService
    {
        bool Exists(int teamId, int invitedUserId);

        Invitation ByTeamIdAndInvitedUserId(int teamId, int invitedUserId);

        void CreateInvitation(int teamId, int invitedUserId);

        void Deactivate(int id);
    }
}