using System.ComponentModel.DataAnnotations;

namespace EEZBankServer.Models
{
    public class UserAccountInfos
    {
        [Display(Name = "Kullanıcı Numarası: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Display(Name = "Kullanıcı Sistem Numarası: ")]
        [Key]
        public int UserSystemId { get; set; }
        [Display(Name = "Kullanıcı Adı: ")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(30,MinimumLength =2,ErrorMessage ="Karakter sayısı 2 ile 30 arasında olmalı")]
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
        [Required(ErrorMessage ="Bu alan boş bırakılamaz")]
        [Compare("UserPassword",ErrorMessage = "Şifreler uyuşmuyor")]
        public string UserPasswordAgain { get; set; }

    }
}
