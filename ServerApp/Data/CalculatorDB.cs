using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

/*
Add-Migration "CalculatorDB_00002"
Update-Database
*/

namespace ServerApp.Data
{
    public class CalculatorDB : IdentityDbContext
    {
        public CalculatorDB(DbContextOptions<CalculatorDB> options) : base(options) { }

        public DbSet<Settings> Settings { get; set; }
    }
}
