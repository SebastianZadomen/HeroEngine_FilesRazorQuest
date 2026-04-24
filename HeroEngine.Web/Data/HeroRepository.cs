using HeroEngine.Model.Heroes;
using System.Text.Json;
using System.IO;

namespace HeroEngine.Web.Data
{
    public class HeroRepository
    {
        private List<Hero> Heroes;

       private readonly string _path;

        public HeroRepository()
        {
            Heroes = new List<Hero>();

            _path = @"Data/Files/heroes.json";
            //_rutaArchivo = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
            LoadAll();
        }
        public void LoadAll()
        {
            if (!File.Exists(_path))
            {
                Heroes = new List<Hero>();
                return;
            }

            string jsonString = File.ReadAllText(_path);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                Heroes = new List<Hero>();
                return;
            }

            var convertList = JsonSerializer.Deserialize<List<Hero>>(jsonString);
            Heroes = convertList ?? new List<Hero>();
        }
        public void SaveAll(IEnumerable<Hero> heroes)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            string jsonString = JsonSerializer.Serialize(heroes, options);
            File.WriteAllText(_path, jsonString);

        }

        public void Add(Hero hero)
        {
            Heroes.Add(hero);
            SaveAll(Heroes);
        }

        public void Delete(string name)
        {
            var heroToRemove = Heroes.FirstOrDefault(x => x.Name == name);
            if (heroToRemove != null)
            {
                Heroes.Remove(heroToRemove);
                SaveAll(Heroes); 
            }
        }
        public List<Hero> GetAll()
        {
            return Heroes;
        }
    }
}
