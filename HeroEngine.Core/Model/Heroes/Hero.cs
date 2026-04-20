using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using HeroEngine.Model.Ability;
using HeroEngine.Utils;

namespace HeroEngine.Model.Heroes
{
    public abstract class Hero
    {
        public const int DamageBase = 5;
        public const int HealthBase = 100;
        public const int HealthScale = 25;
        public const int ExpBase = 100;
        public const int ExpScale = 20;

        public static int Keys { get; set; } = 0;
        private string _name;
        public string Name { get { return _name; } set {
                _name = ValidateName(value);
            
            }
        }
        public int Defense { get; set; }
        public Skill[] Skills { get; set; } = new Skill[4];
        public int Level { get; set; }

        private int _health;
        public int Health { get { return _health; } set {
                _health = StatsControl(value, HealthMax);         
            }
        }

        private int _exp;

        public int Experience
        {
            get { return _exp; }
            private set { _exp = value; }
        }


        protected int HealthMax => HealthBase + HealthScale * Level;

        protected int ExpMax => ExpBase + (ExpScale * Level);
        public bool IsAlive => Health > 0;
        public int Damage => DamageBase + DamageBase * Level;

        public Hero(string name, int level )
        {
            Name = name;
            Level = level;
            Health = HealthMax;
            Defense = 0;

        }
    
        public Hero(string name) : this(name, 0)
        {
            Defense = 0;
        }

        public virtual bool Attack(Hero target, CombatLog log)
        {
            if (!IsAlive)
            {
                log.LogMessage($"{Name} está muerto y no puede atacar.");
                return false;
            }
            return true;
        }
        public virtual bool TakeDamage(int damage, CombatLog log)
        {
            if (!IsAlive)
            {
                log.LogMessage($"{Name} está muerto y no puede recibir daño.");
                return false;
            }
            return true;
        }

        public int DamageCritical(CombatLog log)
        {
            var random = new Random();
            int damageCritical = random.Next(0, 2) == 1 ? Damage * 2 : Damage;

            log.LogMessage(damageCritical.Equals(Damage) ? $"Inflige {damageCritical} de daño.(Golpe normal)" : $"Inflige {damageCritical} de daño.(GolpeCritico)");
            return damageCritical;
        }
        public void AddExperience(int expAdd)
        {
            _exp += expAdd; 

           
            while (_exp >= ExpMax)
            {
                _exp -= ExpMax; 
                Level++;
                Console.WriteLine($"¡{Name} ha subido al nivel {Level}!");
                Health = HealthMax; 
            }
        }

        protected int StatsControl(int valueInput, int maxValue)
        {
            return Math.Clamp(valueInput, 0, maxValue);

        }
        protected string ValidateName(string name)
        {
       
            string result = "";
            foreach (char i in name)
            {

                if (i >= 'a' && i <= 'z' || i >= 'A' && i <= 'Z')
                {
                    result += i;
                }
            }
            if (result.Equals("") || result.Equals(" "))
            {
                Console.WriteLine("Nombre no valido , Registrado como NoName");
                result = "NoName";
            }
            else
            {
                result = result.Substring(0, 1).ToUpper() + result.Substring(1, result.Length - 1).ToLower();
            }
            return result;
            
        }


        public void ShowSkills()
        {
            int totalSlots = Skills.Length; 

            for (int i = 0; i < totalSlots; i++) Console.Write("╔════════════════════════════╗  ");
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++) Console.Write($"║           SLOT {i + 1}           ║  ");
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++) Console.Write("╠════════════════════════════╣  ");
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++)
            {
                string typeText = Skills[i] != null ? $"[{Skills[i].Type}][{Skills[i].Rarity}]" : "[ VACÍO ]";
                Console.Write($"║ {typeText.PadRight(26)} ║  "); 
            }
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++)
            {
                string nameText = Skills[i] != null ? Skills[i].Name : "Ninguna";
                Console.Write($"║ {nameText.PadRight(26)} ║  ");
            }
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++)
            {
                string effectText = Skills[i] != null ? Skills[i].GetSkillEffect() : "";
                Console.Write($"║ {effectText.PadRight(26)} ║  ");
            }
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++)
            {
                string costText = Skills[i] != null ? $"Cost: {Skills[i].CalculatedCost(Skills[i].Rarity)}" : "";
                Console.Write($"║ {costText.PadRight(26)} ║  ");
            }
            Console.WriteLine();

            for (int i = 0; i < totalSlots; i++) Console.Write("╚════════════════════════════╝  ");
            Console.WriteLine();

        }

        public void UseSkills(Hero target, CombatLog log)
        {
            OrderSkills();
            ShowSkills();
            Console.WriteLine("\nSeleccione un Slot de Habilidad (1-4): ");

            int num = 0;
            bool validSelection = false;
            do
            {
                bool validate = int.TryParse(Console.ReadLine(), out num);

                if (!validate || num < 1 || num > 4)
                {
                    Console.WriteLine("Número incorrecto, elige un slot del 1 al 4...");
                }
                else
                {
                    int index = num - 1;

                    if (Skills[index] == null)
                    {
                        log.LogMessage("Está vacío, no haces nada.");
                        validSelection = true;
                    }
                    else
                    {
                        Skills[index].AbilityActivation(target, this, log);
                        validSelection = true;
                    }
                }
            } while (!validSelection);
        }


        public void OrderSkills()
        {
            for (int i = 0; i < Skills.Length - 1; i++)
            {
                for (int j = Skills.Length - 1; j > i; j--)
                {
                    if (Skills[j - 1] == null || (Skills[j] != null && Skills[j].Rarity > Skills[j - 1].Rarity))
                    {
                        Skill temp = Skills[j];
                        Skills[j] = Skills[j - 1];
                        Skills[j - 1] = temp;
                    }
                }
            }
        }

        public void AddSkill(Skill skill)
        {
            for (int i = 0; i < Skills.Length; i++)
            {
                if (Skills[i] != null && Skills[i].Name == skill.Name)
                {
                    Console.WriteLine($"Ya tienes equipada la habilidad: {skill.Name}");
                    return;
                }
            }

            bool skillAdded = false;
            for (int i = 0; i < Skills.Length; i++)
            {
                if (Skills[i] == null)
                {
                    Skills[i] = skill;
                    skillAdded = true;
                    Console.WriteLine($"Has equipado: {skill.Name}");
                    i = Skills.Length + 1; 
                }
            }

            if (!skillAdded)
            {
                Console.Clear();
                Console.WriteLine("Tu barra de habilidades está llena. Quieres sobreescribir alguna habilidad (1-2 cualquier otra opcion sera tomada como no): \n1.Si\n2.No");
                bool Op = (Console.ReadLine() == "1");
                if (Op)
                {
                    OverwriteSkills(skill);
                }
                
            }

            OrderSkills();
        }

        public void OverwriteSkills(Skill skill)
        {
            int op = 0;
            do
            {
                Console.Clear();
                ShowSkills();
                bool validateNum = int.TryParse(Console.ReadLine(), out op);
                if (op < 0 || op > 4)
                {
                    Console.WriteLine("Error : opcion invalida ");
                }
            } while (op < 0 || op > 4);

            op -= 1;
            Skills[op] = skill;    
            

        }

        public override string ToString()
        {
            return $"[{GetType().Name}] Name : {Name} | Level : {Level} | Health : {Health}/{HealthMax}  | Damage : {Damage}";
        }
    }
}
