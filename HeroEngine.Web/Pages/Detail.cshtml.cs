using HeroEngine.Model.Heroes;
using HeroEngine.Web.Pages.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeroEngine.Web.Pages
{
    public class DetailModel : PageModel
    {
        public Hero HeroSelected { get; set; }
   

        public void OnGet(string name) 
        {
            HeroSelected = GameData.HeroRegister.FirstOrDefault(x => x.Name == name);

        }
    }
}
