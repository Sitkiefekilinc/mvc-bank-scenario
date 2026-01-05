using System.ComponentModel.DataAnnotations;

namespace EEZBankServer.Models.ViewModel
{
    public class HesapOluşturViewModel
    {
        [Display(Name = "Hesap İsimlendirme")]
        [StringLength(30, ErrorMessage = "Hesap adı en fazla 30 karakter olabilir.")]
        public string? AccountName { get; set; }

        [Display(Name = "Para Birimi")]
        [Required(ErrorMessage = "Lütfen bir para birimi seçiniz.")]
        public string CurrencyCode { get; set; } = "TRY"; 

        [Range(typeof(bool), "true", "true", ErrorMessage = "Devam etmek için sözleşmeyi onaylamalısınız.")]
        public bool AcceptAgreements { get; set; }
    }
}
