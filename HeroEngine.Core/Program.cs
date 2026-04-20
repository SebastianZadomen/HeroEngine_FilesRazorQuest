using HeroEngine.Model.Ability;
using HeroEngine.Model.Enemy;
using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
public class HeroEngineProgram
{
    public static int CountPlayer = 0;

    public static void Main()
    {
        const string Title = @"
        =====================================================
        ||                                                 ||
        ||       T H E   P R I M O R D I A L   B U G       ||
        ||                                                 ||
        =====================================================";

        const string WelcomeMsg = @"
El kingdom de Bytecroft ha sido invadido por las fuerzas del Bug Primordial.
Necesitan de tu ayuda , elige tu clase : ";

        const string ClassMenu = @"
[1] WARRIOR
[2] MAGE   
[3] ROGUE   ";

        const string Menu = @"
        =====================================================
        ||                                                 ||             
        ||                    M E N U                      || 
        ||                                                 || 
        =====================================================
        ||                                                 ||
        ||  [1]  Ir a pelear (Combate 2v2 por oleada)      ||
        ||                                                 ||
        ||  [2]  Reclutar nuevo Héroe                      ||
        ||                                                 ||
        ||  [3]  Ver Habilidades del Equipo                ||
        ||                                                 ||
        ||  [4]  Abrir Caja de Botín (Requiere 1 Llave)    ||
        ||                                                 ||
        ||  [5]  Abandonar la aventura                     ||
        ||                                                 ||
        ====================================================";

        const string MsgCreateSucces = @"
        =====================================================
        ||                                                 ||             
        ||        {0} Has jurado proteger el             || 
        ||                                                 || 
        ||             mundo de ByteCrof                   || 
        ||                                                 || 
        =====================================================";

        const string PressContinue = "Presiona una tecla para continuar.....";

        Console.WriteLine(Title);
        Console.WriteLine(WelcomeMsg);
        Hero[] teamPlayer = new Hero[4];
        Console.WriteLine(PressContinue);
        


        Hero prota = CreateCharacter(ClassMenu);
        teamPlayer[0] = prota;
        AttackSkills atkBase = new AttackSkills("Ataque basico");
        CountPlayer++;
        Console.WriteLine(MsgCreateSucces,prota.Name);
        Console.ReadKey();

  
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine(Menu);
            Console.Write("\n¿Qué deseas hacer?: ");
            string op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    StartCombat(teamPlayer);
                    break;
                case "2":
                    if (CountPlayer < 4)
                    {
                        CountPlayer++;
                        teamPlayer[CountPlayer - 1] = CreateCharacter(ClassMenu);
                    }
                    else
                    {
                        Console.WriteLine("\n[Sistema] ¡Tu equipo ya está lleno! (Máximo 4 héroes).");
                        Console.ReadLine();
                    }

                    break;
                case "3":
                    ShowTeamSkills(teamPlayer);
                    break;
                case "4":
                    
                 
                    OpenLootBox(teamPlayer);
                    break;
                case "5":
                    isRunning = false;
                    Console.WriteLine("\nTe retiras a descansar... ¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("\n[Sistema] Elige una opción válida.");
                    Console.ReadLine();
                    break;
            }
        }
       
    }
    public static Hero CreateCharacter(string msg)
    {
        Console.WriteLine(msg);
        Hero newHero = null;
        bool valid = false;
        AttackSkills atkBase = null;
        while (!valid)
        {
            Console.Write("\nSelecciona tu clase (1-3): ");
            string classChoice = Console.ReadLine();

            Console.Write("¿Cuál es el nombre del héroe?: ");
            string nameChoice = Console.ReadLine();

           
           switch (classChoice)
            {
                case "1":
                    newHero = new Warrior(nameChoice, 1);
                    atkBase = new AttackSkills("Golpe de espada", RarityType.Comun, 15, 5, 20);
                    valid = true;
                    break;
                case "2":
                    newHero = new Mage(nameChoice, 1);
                    atkBase = new AttackSkills("Bola de fuego", RarityType.Comun, 20, 8, 15);
                    valid = true;
                    break;
                case "3":
                    newHero = new Rogue(nameChoice, 1);
                    atkBase = new AttackSkills("Ataque rapido", RarityType.Comun, 12, 3, 25);
                    valid = true;
                    break;
                default:
                    Console.WriteLine("Clase rechazada, intenta de nuevo.");
                    break;
            }
        
        }
        Console.WriteLine($"\n¡ {newHero.Name}  se ha unido a tus filas! \n Presiona una tecla para continuar.....");
        Console.ReadKey();
        
        newHero.AddSkill(atkBase);
        return newHero;
    }
    public static void StartCombat(Hero[] playerTeam)
    {
        string path = @"..\..\..\Files\CombatLog.txt";

        if (CountPlayer == 0)
        {
            Console.WriteLine("¡No puedes ir a la batalla sin héroes!");
            Console.ReadLine();
            return;
        }

        Console.Clear();
        Console.WriteLine("¡TE ACERCAS AL CAMPO DE BATALLA! \n Recuerda que si tu equipo son mas de dos pelearas contra un jefe final ve preparado");

        CombatLog logger = new CombatLog();
        CombatUtils.ResetStats();

        int enemyCount = CountPlayer * 2;
        Enemy[] enemies = new Enemy[enemyCount];
        bool bossActive = CountPlayer >= 2;

        Random rnd = new Random();
        for (int i = 0; i < enemyCount; i++)
        {
            if (i == enemyCount - 1 && bossActive)
            {
                string bossName = GetRandomName("BOSS", rnd);
                enemies[i] = new Boss(bossName, 5);

                enemies[i].Skills[0] = new AttackSkills("Golpe de Jefe", RarityType.Comun, 8, 15, 20);
                enemies[i].Skills[1] = new AttackSkills("Ira del Abismo", RarityType.Raro, 15, 8, 45);
                enemies[i].Skills[2] = new AttackSkills("Devastación Oscura", RarityType.Epico, 20, 5, 60);
                enemies[i].Skills[3] = new SupportSkills("Regeneración Oscura", RarityType.Raro, 15, 3, 30);
            }
            else
            {
                if (i % 2 == 0)
                {
                    string minionName = GetRandomName("MINION", rnd);
                    enemies[i] = new Minion(minionName, 0);

                    enemies[i].Skills[0] = new AttackSkills("Arañazo", RarityType.Comun, 5, 15, 10);
                    enemies[i].Skills[1] = new AttackSkills("Golpe Cargado", RarityType.Comun, 5, 15, 12);
                }
                else
                {
                    string eliteName = GetRandomName("ELITE", rnd);
                    enemies[i] = new Elite(eliteName, 2);

                    enemies[i].Skills[0] = new AttackSkills("Ataque Fuerte", RarityType.Comun, 8, 10, 18);
                    enemies[i].Skills[1] = new AttackSkills("Cuchilla Mortal", RarityType.Raro, 12, 5, 35);
                }
            }
        }


        Hero[] activeHeroes = new Hero[2];
        Enemy[] activeEnemies = new Enemy[2];

        int round = 1;

        while (HasAliveEntities(playerTeam, CountPlayer) && HasAliveEntities(enemies, enemyCount))
        {
            logger.LogRoundStart(round);

            FillHeroes(playerTeam, activeHeroes, CountPlayer);
            FillEnemies(enemies, activeEnemies, enemyCount, logger);

            for (int slot = 0; slot < 2; slot++)
            {
                if (activeHeroes[slot] != null && activeHeroes[slot].IsAlive)
                {
                    Console.WriteLine($"\n--- Turno de {activeHeroes[slot].Name} ---");

                    Console.WriteLine("Enemigos en el campo:");
                    for (int e = 0; e < activeEnemies.Length; e++)
                    {
                        if (activeEnemies[e] != null && activeEnemies[e].IsAlive)
                        {
                            Console.WriteLine($"[{e + 1}] {activeEnemies[e].Name} (HP: {activeEnemies[e].Health})");
                        }
                    }

                    Enemy target = null;
                    while (target == null)
                    {
                        Console.Write("\nElige a qué enemigo atacar (número): ");
                        if (int.TryParse(Console.ReadLine(), out int op) && op > 0 && op <= activeEnemies.Length)
                        {
                            int index = op - 1;
                            if (activeEnemies[index] != null && activeEnemies[index].IsAlive)
                            {
                                target = activeEnemies[index];
                            }
                            else
                            {
                                Console.WriteLine("Ese enemigo no es válido o ya ha sido derrotado. Intenta de nuevo.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Opción incorrecta. Introduce el número de un enemigo de la lista.");
                        }
                    }

                    if (target != null)
                    {
                        Console.WriteLine($"\nEnemigo objetivo: {target.Name} (HP: {target.Health})");

                        int lifeBefore = target.Health;

                        activeHeroes[slot].UseSkills(target, logger);


                        Console.Clear();
                        logger.LogRoundStart(round);

                        bool killed = !target.IsAlive;


                        logger.LogAction("HERO", activeHeroes[slot].Name, "Acción de combate", target.Name, lifeBefore-target.Health, killed);
                        logger.LogMessage("======================================================================");

                        if (killed)
                        {
                            CombatUtils.RegisterEnemyDefeat(target.Name, round);

                            if (target.DropKey())
                            {
                                Hero.Keys++;
                                Console.WriteLine($@"
======================================
🗝️ ¡{target.Name} ha soltado una LLAVE!
🗝️ Llaves totales: {Hero.Keys}
======================================");
                            }
                            else
                            {
                                Console.WriteLine($"\n💀 {target.Name} ha sido derrotado (No soltó nada).");
                            }
                        }
                    }
                }

       
                if (activeEnemies[slot] != null && activeEnemies[slot].IsAlive)
                {
                    Hero targetHero = GetFirstAliveHero(activeHeroes);
                    if (targetHero != null)
                    {

                        int lifeHeroBefore = targetHero.Health;

                        activeEnemies[slot].ActionsPerTurn(activeHeroes, logger);

                        logger.LogAction("ENEMY", activeEnemies[slot].Name, "Acción de combate", targetHero.Name, lifeHeroBefore-targetHero.Health, !targetHero.IsAlive);
                    }
                }
            }

            int enemiesLeft = CountAliveEntities(enemies, enemyCount);
            int heroesLeft = CountAliveEntities(playerTeam, CountPlayer);

            logger.LogRoundEnd(enemiesLeft, heroesLeft);
            round++;
            logger.SaveToFile(path);
            Console.WriteLine("\nPulsa Enter para la siguiente ronda...");
            Console.ReadKey();
        }

        Console.Clear();
        if (HasAliveEntities(playerTeam, CountPlayer))
        {
            Console.WriteLine("¡LA VICTORIA ES TUYA! Haz logrado sobrevivir a las oleadas.");
        }
        else
        {
            Console.WriteLine("DERROTA... Tus héroes han caído ante la oscuridad.");
        }

        CombatUtils.ShowCombatStats();
        logger.SaveToFile(path);

        Console.ReadKey();
    }
    public static string GetRandomName(string type, Random rnd)
    {
        string[] minionNames = { "Ciego Devoto", "Mártir del Látigo", "Penitente", "Querubín Menor", "Esqueleto Arquero", "Locusta" };
        string[] eliteNames = { "Serafín de Seis Alas", "Guerrero Ungido", "Heraldo del Juicio", "Dominación", "Cerbero", "Valkyria" };
        string[] bossNames = { "Metatrón, la Voz de Dios", "Abadón, el Ángel del Abismo", "Escribar, el Último Hijo", "Lucifuge Rofocale" };

        if (type == "MINION") return minionNames[rnd.Next(minionNames.Length)];
        if (type == "ELITE") return eliteNames[rnd.Next(eliteNames.Length)];
        return bossNames[rnd.Next(bossNames.Length)]; 
    }
    public static void ShowTeamSkills(Hero[] playerTeam)
    {
        Console.Clear();
        Console.WriteLine("=== HABILIDADES DEL EQUIPO ===");
        for (int i = 0; i < CountPlayer; i++)
        {
            if (playerTeam[i] != null)
            {
                Console.WriteLine($"\n--- Héroe: {playerTeam[i].Name} ---");
                playerTeam[i].ShowSkills(); 
            }
        }
        Console.WriteLine("\nPresiona Enter para volver...");
        Console.ReadKey();
    }
    public static bool HasAliveEntities(Hero[] array, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (array[i] != null && array[i].IsAlive) return true;
        }
        return false;
    }

    public static int CountAliveEntities(Hero[] array, int count)
    {
        int aliveCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (array[i] != null && array[i].IsAlive) aliveCount++;
        }
        return aliveCount;
    }

    public static void FillHeroes(Hero[] reserves, Hero[] active, int reserveCount)
    {
        for (int i = 0; i < 2; i++)
        {
            if (active[i] == null || !active[i].IsAlive)
            {
                for (int j = 0; j < reserveCount; j++)
                {
                    if (reserves[j] != null && reserves[j].IsAlive && reserves[j] != active[0] && reserves[j] != active[1])
                    {
                        active[i] = reserves[j];
                        Console.WriteLine($"{active[i].Name} entra al campo de batalla.");
                        break;
                    }
                }
            }
        }
    }

    public static void FillEnemies(Enemy[] reserves, Enemy[] active, int reserveCount, CombatLog logger)
    {
        for (int i = 0; i < 2; i++)
        {
            if (active[i] == null || !active[i].IsAlive)
            {
                for (int j = 0; j < reserveCount; j++)
                {
                    if (reserves[j] != null && reserves[j].IsAlive && reserves[j] != active[0] && reserves[j] != active[1])
                    {
                        active[i] = reserves[j];
                        logger.LogAction("SISTEMA", active[i].Name, "Aparece desde las sombras", "Campo", 0);
                        break; 
                    }
                }
            }
        }
    }

    public static Enemy GetFirstAliveEnemy(Enemy[] activeEnemies)
    {
        if (activeEnemies[0] != null && activeEnemies[0].IsAlive) return activeEnemies[0];
        if (activeEnemies[1] != null && activeEnemies[1].IsAlive) return activeEnemies[1];
        return null;
    }

    public static Hero GetFirstAliveHero(Hero[] activeHeroes)
    {
        if (activeHeroes[0] != null && activeHeroes[0].IsAlive) return activeHeroes[0];
        if (activeHeroes[1] != null && activeHeroes[1].IsAlive) return activeHeroes[1];
        return null;
    }


    public static void OpenLootBox(Hero[] teamPlayer)
    {
        Console.Clear();
        Console.WriteLine($"\nTienes {Hero.Keys} llaves místicas.");

        if (Hero.Keys <= 0)
        {
            Console.WriteLine("No tienes llaves suficientes. ¡Ve a combatir!");
            Console.ReadLine();
            return;
        }

        Console.Write("¿Deseas gastar 1 llave para abrir la caja? (s/n): ");
        if (Console.ReadLine().ToLower() == "s")
        {
            Hero.Keys--;
            Random rnd = new Random();

            Skill[] lootPool = new Skill[]
            {
            new AttackSkills("Tajo Básico"),
            new AttackSkills("Estocada Rápida"),
            new AttackSkills("Golpe Contundente"),
            new AttackSkills("Corte Sangrante"),
            new AttackSkills("Ira del Guerrero"),
            new AttackSkills("Fuego Sagrado"),
            new AttackSkills("Tormenta de Espadas"),
            new AttackSkills("Golpe Devastador"),
            new AttackSkills("Juicio Final"),
            new AttackSkills("Meteoro"),
            
            new SupportSkills("Poción Menor"),
            new SupportSkills("Primeros Auxilios"),
            new SupportSkills("Luz Curativa"),
            new SupportSkills("Regeneración Vital"),
            new SupportSkills("Aliento de Vida"),
            
            new DefenseSkills("Bloqueo Básico"),
            new DefenseSkills("Postura Defensiva"),
            new DefenseSkills("Piel de Hierro"),
            new DefenseSkills("Muro de Piedra"),
            new DefenseSkills("Égida Divina")
            };

            Skill skillToca = lootPool[rnd.Next(lootPool.Length)];

            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine($"✨ ¡CAJA ABIERTA! ✨");
            Console.WriteLine($"Has obtenido: [{skillToca.Rarity}] {skillToca.Name}");
            Console.WriteLine("========================================\n");

            Console.WriteLine("¿A qué héroe le quieres dar la habilidad?");
            ShowPlayer(teamPlayer);

            bool validChoice = false;
            while (!validChoice)
            {
                Console.Write("\nElige el número del héroe: ");
                if (int.TryParse(Console.ReadLine(), out int op) && op >= 0 && op < CountPlayer)
                {
                    if (teamPlayer[op] != null)
                    {
                        teamPlayer[op].AddSkill(skillToca);
                        validChoice = true;
                    }
                }

                if (!validChoice)
                {
                    Console.WriteLine("Opción no válida. Intenta de nuevo.");
                }
            }
        }

        Console.WriteLine("\nPresiona Enter para volver al menú...");
        Console.ReadKey();
    }
    public static void ShowPlayer(Hero[] teamPlayer)
    {
        int count = 0;
        foreach (var pj in teamPlayer)
        {
            if (pj != null)
            {
                Console.WriteLine($"{count}. [{pj.GetType().Name}] {pj.Name}");
            }
            count++;
        }
    }
}
