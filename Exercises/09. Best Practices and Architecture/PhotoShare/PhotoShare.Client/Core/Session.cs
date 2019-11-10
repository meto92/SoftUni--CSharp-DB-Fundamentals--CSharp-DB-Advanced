using PhotoShare.Models;

namespace PhotoShare.Client.Core
{
    public static class Session
    {
        public static User User { get; set; }

        public static bool IsUserLoggedIn => User != null;
    }
}