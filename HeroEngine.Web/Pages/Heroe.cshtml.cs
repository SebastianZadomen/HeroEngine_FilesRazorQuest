using HeroEngine.Model.Heroes;
using HeroEngine.Web.Pages.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeroEngine.Web.Pages
{
    public class HeroeModel : PageModel
    {
        public List<Hero> HeroRegister { get; set; }
        public void OnGet()
        {
            this.HeroRegister = GameData.HeroRegister;
        }
    }
}

 