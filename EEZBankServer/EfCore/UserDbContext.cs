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
        public DbSet<KurumsalKullaniciModel> KurumsalKullaniciBilgileri { get; set; }
        public DbSet<TicariKullaniciModel> TicariKullaniciBilgileri { get; set; }
        public DbSet<BankAccountsModel> Hesaplar { get; set; }

        public DbSet<IslemlerModel> Islemler { get; set; }
    }
}
