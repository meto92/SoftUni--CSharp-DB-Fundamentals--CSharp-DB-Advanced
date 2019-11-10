using TeamBuilder.Data;
using TeamBuilder.Services.Interfaces;

namespace TeamBuilder.Services
{
    public class DatabaseInitializerService : IDatabaseInitializerService
    {
        private readonly TeamBuilderContext db;

        public DatabaseInitializerService(TeamBuilderContext db)
        {
            this.db = db;
        }

        public void InitializeDatabase()
        {
            db.Database.EnsureCreated();
        }
    }
}