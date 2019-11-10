using BusTicketsSystem.Data;
using BusTicketsSystem.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BusTicketsSystem.Services
{
    public class DatabaseInitializerService : IDatabaseInitializerService
    {
        private readonly BusTicketsSystemContext db;

        public DatabaseInitializerService(BusTicketsSystemContext context)
        {
            this.db = context;
        }

        public void InitializeDatabase() => this.db.Database.Migrate();
    }
}