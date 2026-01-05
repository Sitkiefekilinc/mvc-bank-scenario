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

        [NotMapped]
        public string FullName => $"{UserName} {UserSurname}";

        [Display(Name = "E-Posta Adresi: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [EmailAddress]
        public string UserEmail { get; set; }
        
        [Display(Name = "Kullanıcı Şifresi: ")]
        [Required]
        public string UserPassword { get; set; }
        [Display(Name = "Kullanıcı Şifresi Tekrar: ")]
        [Required(ErrorMessage ="Bu alan boş bırakılamaz")]
        [Compare("UserPassword",ErrorMessage = "Şifreler uyuşmuyor")]
        [NotMapped]
        public string UserPasswordAgain { get; set; }

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
