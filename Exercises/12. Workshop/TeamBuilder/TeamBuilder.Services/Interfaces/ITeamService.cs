namespace TeamBuilder.Services.Interfaces
{
    public interface ITeamService
    {
        TModel ByName<TModel>(string teamName);

        bool Exists(string teamName);

        bool IsUserCreatorOfTeam(string teamName, int userId);

        bool IsUserMemberOfTeam(string teamName, int userId);

        void CreateTeam(string teamName, int creatorId, string acronym, string description);

        void AddMember(int teamId, int userId);

        void KickMember(string teamName, int userToBeKickedId);

        void Disband(string teamName);
    }
}