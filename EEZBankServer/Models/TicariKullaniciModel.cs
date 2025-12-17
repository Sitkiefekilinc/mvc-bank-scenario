using System.ComponentModel.DataAnnotations;

namespace EEZBankServer.Models
{
    public class TicariKullaniciModel
    {
        [Key]
        public  Guid TicariId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public UserAccountInfos User { get; set; }

        [Display(Name="Şirket İsmi")]
        [Required(ErrorMessage ="Şirket İsmi Girilmelidir")]
        public string CompanyName { get; set; }
        [Display(Name = "Şirket E-Postası")]
        [Required(ErrorMessage = "Şirket E-Postası Girilmelidir")]
        public string CompanyEmail { get; set; }
    }
}
