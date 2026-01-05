using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEZBankServer.Models
{
    public class BankAccountsModel
    {
        [Key]
        public Guid HesapId { get; set; } = new Guid();

        [Required]
        [Display(Name = "Hesap Numarası")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Hesap numarası tam 10 karakter olmalıdır.")]

        public string AccountNumbers { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public UserAccountInfos User { get; set; }

        [Display(Name = "IBAN Numarası")]
        [Required(ErrorMessage = "IBAN alanı zorunludur.")]
        [StringLength(26, MinimumLength = 26, ErrorMessage = "IBAN numarası tam 26 karakter olmalıdır.")]
        [Column(TypeName = "char(26)")]
        public string Iban { get; set; }
        [Display(Name = "Bakiye")]
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        [Display(Name = "Para Birimi")]
        [Required]
        [StringLength(3, ErrorMessage = "Para birimi kodu 3 karakter olmalıdır (Örn: TRY).")]

        public string CurrencyCode { get; set; }

        [Display(Name = "Hesap Adı")]
        [StringLength(50, ErrorMessage = "Hesap adı en fazla 50 karakter olabilir.")]


        public string? AccountName { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
