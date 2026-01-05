using System.ComponentModel.DataAnnotations;

namespace EEZBankServer.Models.ViewModel
{
    public class TransferFaturaOdeViewModel
    {
        [Required(ErrorMessage = "Ödeme yapılacak hesabı seçiniz.")]
        public Guid GonderenHesapId { get; set; }

        [Required(ErrorMessage = "Lütfen kurum seçiniz.")]
        public string KurumAdi { get; set; } 

        [Required(ErrorMessage = "Abone numarası zorunludur.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Geçerli bir abone no giriniz.")]
        public string AboneNo { get; set; }

        [Required(ErrorMessage = "Fatura tutarı zorunludur.")]
        [Range(1, 100000, ErrorMessage = "Geçerli bir tutar giriniz.")]
        public decimal Tutar { get; set; }

        public List<BankAccountsModel>? KullaniciHesaplari { get; set; }
    }
}
