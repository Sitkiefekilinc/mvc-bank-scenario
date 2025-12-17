using EEZBankServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.ViewComponents
{
    public class DovizKurlariViewComponent : ViewComponent
    {
        private readonly IDovizService _dovizService;

        public DovizKurlariViewComponent(IDovizService dovizService)
        {
            _dovizService = dovizService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dovizKurlari = await _dovizService.GetSonFiyatlariAl();
            return View(dovizKurlari);
        }
    }
}
