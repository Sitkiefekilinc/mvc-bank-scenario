namespace EEZBankServer.Models.ViewModel
{
    public class ProfileViewModel
    {
        public UserAccountInfos UserAccountInfos { get; set; }
        public List<BankAccountsModel> Hesaplar { get; set; }
    }
}
