using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEZBankServer.Models
{
    public class TicariKullaniciModel
    {
        [Key]
        public  Guid TicariId { get; set; } = Guid.NewGuid();
        [ForeignKey("Users")]
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
