using TeamBuilder.App.Core.Interfaces;
using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core.Commands
{
    public class LogoutCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            string result = string.Format(
                Constants.SuccessMessages.Logout,
                AuthenticationManager.GetCurrentUser().Username);

            AuthenticationManager.Logout();

            return result;
        }
    }
}