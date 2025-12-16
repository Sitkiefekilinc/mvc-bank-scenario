using EEZBankServer.Models;
using Microsoft.EntityFrameworkCore;
namespace EEZBankServer.EfCore
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            
        }
        public DbSet<UserAccountInfos> Users { get; set; }
    }
}
