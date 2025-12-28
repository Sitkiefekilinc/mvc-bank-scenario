using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEZBankServer.Models
{
    public class KurumsalKullaniciModel
    {
        [Key]
        public Guid KurumsalId  { get; set; } = Guid.NewGuid();
        [ForeignKey("Users")]
        public Guid UserId { get; set; }
        public UserAccountInfos User { get; set; }

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
    }
    public enum CorporateTypes
    {
        [Display(Name = "Eğitim ve Sağlık Kurumu")]
        EducationAndHealth,
        [Display(Name = "Kamu Kurumu")]
        PublicInstitution,
        [Display(Name = "Vakıf / Dernek")]
        Foundation,
        [Display(Name = "Özel Şirket (LTD/AŞ)")]
        PrivateCompany
    }
}
