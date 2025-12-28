using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEZBankServer.Models
{
    public class IslemlerModel
    {
        [Key]
        [Required]
        public Guid IslemId { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("HesapId")]
        public Guid GonderenHesapId { get; set; }
        public virtual BankAccountsModel GonderenBankaHesabi { get; set; }

        [Required]
        [ForeignKey("HesapId")]
        public Guid? AliciHesapId { get; set; }
        public virtual BankAccountsModel? AliciBankaHesabi { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal IslemMiktari { get; set; }
        public string? Aciklama { get; set; }

        [Required]
        public IslemTuru Tur { get; set; } 

        [Required]
        public DateTime IslemTarihi { get; set; } = DateTime.Now;

    }

    public enum IslemTuru
    {
        Havale,
        EFT,
        Odeme,
        Yatirim
    }
}
