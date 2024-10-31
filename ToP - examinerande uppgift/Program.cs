using System.Runtime.Intrinsics.Arm;

namespace ToP___examinerande_uppgift
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal class Program
    {
        // Counters
        private static int medborgareRobbed = 0;
        private static int tjuvCaught = 0;

        static void Main()
        {
            Console.CursorVisible = false;

            char[,] city = CityManager.CreateInnerCity();
            CityManager.CreateCityWalls(city);

            List<Person> people = PeopleManager.AddPeople();
            PeopleManager.AddToTillhörigheter(people);
            printCounters();

            // Main loop that can be stopped by pressing the escape key
            while (true)
            {
                Thread.Sleep(500);
                PeopleManager.ClearPosition(city, people);
                PeopleManager.MovePeople(people);
                CollisionManager.CheckCollisions(people, ref medborgareRobbed, ref tjuvCaught);
                PeopleManager.UpdatePeoplePosition(city, people);
                CityManager.CreateCityWalls(city);
                printCounters();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Program avslutades");
                        break;
                    }
                }
            }
        }

        public static void printCounters()
        {
            Console.SetCursorPosition(0, CityManager.GetCityHeight() + 1);
            Console.WriteLine($"Antal rånade medborgare: {medborgareRobbed}");
            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(0, CityManager.GetCityHeight() + 2);
            Console.WriteLine($"Antal gripna tjuvar: {tjuvCaught}");
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }
}


/*
static string GenerateName()
{
    string[] Names = { "Nelly", "Ebba", "Wera", "Christin", "Astrid", "Ylva", "Anthony", "Mikael", "Sean", "Nikita", "Arya", "Jakob", "Natan", "Joel", "Noa", "Melker" };
    return Names[rndm.Next(Names.Length)];
}*/