using System.ComponentModel.DataAnnotations;

namespace EEZBankServer.Models.ViewModel
{
    public class TransferEftViewModel
    {

         [Required(ErrorMessage = "Lütfen gönderim yapacağınız hesabı seçiniz.")]
         public Guid GonderenHesapId { get; set; }

         [Required(ErrorMessage = "Alıcı IBAN numarası zorunludur.")]
         [StringLength(26, MinimumLength = 26, ErrorMessage = "IBAN 26 karakter olmalıdır (TR dahil).")]
         public string AliciIban { get; set; }

         [Required(ErrorMessage = "Alıcı Ad Soyad zorunludur.")]
         public string AliciAdSoyad { get; set; }

         [Required(ErrorMessage = "Tutar zorunludur.")]
         [Range(20, 1000000, ErrorMessage = "En az 20 TL gönderebilirsiniz.")]
         public decimal Tutar { get; set; }

         public string? Aciklama { get; set; }

         public List<BankAccountsModel>? KullaniciHesaplari { get; set; }

    }
}

