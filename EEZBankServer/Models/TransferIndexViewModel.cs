namespace EEZBankServer.Models
{
    public class TransferIndexViewModel
    {
        public List<BankAccountsModel> Hesaplar { get; set; }
        public List<IslemlerModel> SonIslemler { get; set; }
    }
}
