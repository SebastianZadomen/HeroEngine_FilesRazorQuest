using HeroEngine.Model.Heroes;
using HeroEngine.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeroEngine.Web.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string HeroName { get; set; }

        [BindProperty]
        public int HeroLevel { get; set; }

        [BindProperty]
        public string HeroClass { get; set; }
        public void OnGet()
        {
            HeroLevel = 1;
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(HeroName) || HeroLevel < 0)
            {
                return Page();
            }

            Hero newHero;

            switch (HeroClass)
            {
                case "Warrior":
                    newHero = new Warrior(HeroName, HeroLevel);
                    break;
                case "Mage":
                    newHero = new Mage(HeroName, HeroLevel);
                    break;
                case "Rogue":
                    newHero = new Rogue(HeroName, HeroLevel);
                    break;
                default:   
                    newHero = new Warrior(HeroName, HeroLevel);
                    break;
            }

            GameData.Repository.Add(newHero);

            return RedirectToPage("/Heroes");
        }
    }
}