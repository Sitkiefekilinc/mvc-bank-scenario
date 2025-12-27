using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEZBankServer.Models
{
    public class UserAccountInfos
    {
        [Key]
        [Display(Name = "Kullanıcı Numarası: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Display(Name = "Kullanıcı Adı: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
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
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Şifreniz en az 8 karakter olmalı")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,}$",
    ErrorMessage = "Şifreniz en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter (.,@,$,!,%,*,?,&) içermelidir.")]

        public string UserPassword { get; set; }
        [Display(Name = "Kullanıcı Şifresi Tekrar: ")]
        [Required(ErrorMessage ="Bu alan boş bırakılamaz")]
        [Compare("UserPassword",ErrorMessage = "Şifreler uyuşmuyor")]
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
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string UserPhoneNumber { get; set; }

        [Display(Name = "Tc Kimlik Numarası")]
        [Required(ErrorMessage = "Tc Kimlik Numarası zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Tc Kimlik Numarası 11 karakter olmalıdır")]
        public string TcKimlikNo { get; set; }

        [Display(Name = "Doğum Tarihi:")]
        [Required(ErrorMessage = "Doğum Tarihi zorunludur")]
        public DateTime UserBirthDate { get; set; }

        [Display(Name = "Adres:")]
        [Required(ErrorMessage ="Adres Alanı Doldurulmalıdır")]
        public string Adress { get; set; }

        [Display(Name = "Kayıt Tarihi:")]
        public DateTime UserCreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Hesap Durumu:")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kullanım Sözleşmesi Onayı:")]
        [Required(ErrorMessage ="KVKK ve sözleşme şartları kabul edilmelidir")]
        public bool HasTheAgreementBeenAccepted { get; set; }

        [Display(Name = "Kullanıcı Tipi:")]
        public UserTypes UserType { get; set; }

    }

    public enum UserTypes
    {
        Bireysel,
        Kurumsal,
        Ticari,
        Admin
    }
}
