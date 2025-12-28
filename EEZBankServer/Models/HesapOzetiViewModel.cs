namespace EEZBankServer.Models
{
    public class HesapOzetiViewModel
    {
        public List<BankAccountsModel> Hesaplar { get; set; }
        public List<IslemlerViewModel> Islemler { get; set; }
        public decimal ToplamBakiye { get; set; }
        

    }
    public enum IslemYonu
    {
        Giden = 0,
        Gelen
    }
    public class IslemlerViewModel
    {
        public string HesapAdi { get; set; }

        public IslemYonu Yon { get; set; }
        public decimal Tutar { get; set; }

        public string KullaniciAdi { get; set; }

        public string KarsiKullaniciAdi { get; set; }

        public DateTime Islemtarihi { get; set; }
    }







}
