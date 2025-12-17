using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EEZBankServer.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Kullanıcı Numarası: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Display(Name = "Kullanıcı Sistem Numarası: ")]
        public string UserName { get; set; }
        [Display(Name = "Kullanıcı Soyadı: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Karakter sayısı 2 ile 30 arasında olmalı")]
        public string UserSurname { get; set; }
        [Display(Name = "E-Posta Adresi: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Display(Name = "Kullanıcı Şifresi: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public string UserPassword { get; set; }
        [Display(Name = "Kullanıcı Şifresi Tekrar: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Compare("UserPassword", ErrorMessage = "Şifreler uyuşmuyor")]
        [NotMapped]
        public string UserPasswordAgain { get; set; }

        [Display(Name = "Hesap Bakiyesi:")]
        [DataType(DataType.Currency)]
        public decimal UserBalance { get; set; }

        [Display(Name = "IBAN Numarası:")]
        [StringLength(26, ErrorMessage = "IBAN 26 karakter olmalıdır")]
        public string UserIban { get; set; }

        [Display(Name = "Telefon Numarası:")]
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string UserPhoneNumber { get; set; }

        [Display(Name = "Tc Kimlik Numarası")]
        [Required(ErrorMessage = "Tc Kimlik Numarası zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Tc Kimlik Numarası 11 karakter olmalıdır")]
        public string TcKimlikNo { get; set; }

        [Display(Name = "Doğum Tarihi:")]
        [Required(ErrorMessage = "Doğum Tarihi zorunludur")]
        public DateTime UserBirthDate { get; set; }

        [Display(Name = "Adres:")]
        [Required(ErrorMessage = "Adres Alanı Doldurulmalıdır")]
        public string Adress { get; set; }

        [Display(Name = "Kayıt Tarihi:")]
        public DateTime UserCreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Hesap Durumu:")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kullanım Sözleşmesi Onayı:")]
        [Required(ErrorMessage = "KVKK ve sözleşme şartları kabul edilmelidir")]
        public bool HasTheAgreementBeenAccepted { get; set; }

        [Display(Name = "Kullanıcı Tipi:")]
        public UserTypes UserType { get; set; }

        public Guid KurumsalId { get; set; } = Guid.NewGuid();

        [Display(Name = "Kurum Adı:")]
        [Required(ErrorMessage = "Kurum adı boş bırakılamaz")]
        public string CorporateName { get; set; }

        [Display(Name = "Kurum Türü:")]
        [Required(ErrorMessage = "Lütfen kurum türünü seçiniz")]
        public CorporateTypes CorporateType { get; set; }

        [Display(Name = "Vergi Numarası:")]
        [Required(ErrorMessage = "Vergi numarası zorunludur")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Vergi numarası 10 haneli olmalıdır")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Sadece rakam girişi yapılabilir")]
        public string TaxNumber { get; set; }

        [Display(Name = "Kurum Adresi:")]
        [Required(ErrorMessage = "Kurumun açık adresi girilmelidir")]
        public string CorporateAddress { get; set; }

        [Display(Name = "Yetkili Kişi Görevi:")]
        [Required(ErrorMessage = "Yetkili Kişinin Görevi Girilmelidir")]
        public string AuthorizedPersonsTask { get; set; }

        public Guid TicariId { get; set; } = Guid.NewGuid();

        [Display(Name = "Şirket İsmi")]
        [Required(ErrorMessage = "Şirket İsmi Girilmelidir")]
        public string CompanyName { get; set; }
        [Display(Name = "Şirket E-Postası")]
        [Required(ErrorMessage = "Şirket E-Postası Girilmelidir")]
        public string CompanyEmail { get; set; }
    }
}
