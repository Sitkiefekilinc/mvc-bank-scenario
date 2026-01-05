using EEZBankServer.Models;
using EEZBankServer.Models.ViewModel;

namespace EEZBankServer.Services
{
    public interface IDovizService
    {
        Task<List<DovizKurlariViewModel>> GetSonFiyatlariAl();
    }

    public class DovizService : IDovizService
    {
        private readonly HttpClient _httpClient;

        public DovizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<DovizKurlariViewModel>> GetSonFiyatlariAl()
        {
            var paraBirimleri = new[] { "USD", "EUR", "GBP" };
            var result = new List<DovizKurlariViewModel>();

            foreach (var code in paraBirimleri)
            {
                var response = await _httpClient.GetFromJsonAsync<FrankfurterDovizModel>($"latest?from={code}&to=TRY");

                if (response != null && response.Rates.ContainsKey("TRY"))
                {
                    decimal ortaOran = response.Rates["TRY"];
                    result.Add(new DovizKurlariViewModel
                    {
                        Code = code,
                        BuyRate = ortaOran * 0.985m,
                        SellRate = ortaOran * 1.015m,
                        UpdateTime = DateTime.Now
                    });
                }
            }
            return result;
        }
    }
}
