using HeroEngine.Model.Heroes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HeroEngine.Model.Ability;
using HeroEngine.Web.Data;

namespace HeroEngine.Web.Pages
{
    public class DetailModel : PageModel
    {
        public Hero HeroSelected { get; set; }
   

        public void OnGet(string name) 
        {
           // HeroSelected = GameData.Repository.;

        }
    }
}
