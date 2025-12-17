namespace EEZBankServer.Models
{
    public class FrankfurterDovizModel
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
