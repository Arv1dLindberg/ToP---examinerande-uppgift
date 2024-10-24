using System.Runtime.Intrinsics.Arm;

namespace ToP___examinerande_uppgift
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal class Program
    {
        // City Size
        private const int cityWidth = 100;
        private const int cityHeight = 25;

        // Counters
        private static int medborgareRobbed = 0;
        private static int tjuvCaught = 0;

        private static Random rndm = new Random();

        static void Main()
        { 
            Console.CursorVisible = false;
            char[,] city = CreateInnerCity();
            CreateCityWalls(city);
            List<Person> people = AddPeople();
            AddToTillhörigheter(people);
            Counters();

            // Loops the program until manually stopped by pressing the escape key
            while (true)
            {
                Thread.Sleep(500);
                ClearPosition(city, people);
                MovePeople(people);
                CheckCollisions(people);
                PeoplePosition(city, people);
                CreateCityWalls(city);
                Counters();

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


        // Creates the city walls
        static void CreateCityWalls(char[,] city)
        {
            Console.Clear();

            Console.WriteLine(new string('X', cityWidth));
            for (int i = 1; i < cityHeight - 1; i++)
            {
                Console.Write('X');

                for (int j = 1; j < cityWidth - 1; j++)
                {
                    Console.Write(city[i, j]);
                }

                Console.WriteLine('X');
            }
            Console.WriteLine(new string('X', cityWidth));
        }


        // Creates the space that the people will move around in
        static char[,] CreateInnerCity()
        {
            char[,] city = new char[cityHeight, cityWidth];

            for (int i = 0; i < cityHeight; i++)
                for (int j = 0; j < cityWidth; j++)
                    city[i, j] = ' ';
            return city;
        }


        // Generates a specified amount of people
        static List<Person> AddPeople()
        {
            List<Person> people = new List<Person>();
            people.AddRange(PlacePeople<Tjuv>(10));
            people.AddRange(PlacePeople<Polis>(15));
            people.AddRange(PlacePeople<Medborgare>(30));
            return people;
        }


        // Adds items to Tillhörigheter
        static void AddToTillhörigheter(List<Person> people)
        {
            foreach (var medborgare in people.OfType<Medborgare>())
            {
                medborgare.Saker.Add(new Sak("Nycklar"));
                medborgare.Saker.Add(new Sak("Mobiltelefon"));
                medborgare.Saker.Add(new Sak("Pengar"));
                medborgare.Saker.Add(new Sak("Klocka"));
            }
        }


        /* Creates and places a specified amount of people within the city and randomly assigns them one of 7 directions
         * (left, right, diagonally left, diagonally right, up, down, and still */
        static List<T> PlacePeople<T>(int count) where T : Person, new()
        {
            List<T> people = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int x = rndm.Next(1, cityWidth - 1);
                int y = rndm.Next(1, cityHeight - 1);

                T person = new T
                {
                    X = x,
                    Y = y,
                    Xdirection = rndm.Next(-1, 2),
                    Ydirection = rndm.Next(-1, 2)
                };
                people.Add(person);
            }
            return people;
        }


        // Gives the person's their direction and moves them
        static void MovePeople(List<Person> people)
        {
            foreach (var person in people)
            {
                person.X += person.Xdirection;

                // If a person moves outside the X walls they will come back on the opposite X-axis
                if (person.X <= 0)
                {
                    person.X = Program.cityWidth - 1;
                }
                else if (person.X >= Program.cityWidth)
                {
                    person.X = 1;
                }

                person.Y += person.Ydirection;

                // If a person moves outside the Y walls they will come back on the opposite Y-axis
                if (person.Y <= 0)
                {
                    person.Y = Program.cityHeight - 1;
                }
                else if (person.Y >= Program.cityHeight)
                {
                    person.Y = 1;
                }
            }
        }


        // Clears the person's previous position
        static void ClearPosition(char[,] city, List<Person> people)
        {
            foreach (var person in people)
                city[person.Y, person.X] = ' ';
        }


        // Updates person's position in the city
        static void PeoplePosition(char[,] city, List<Person> people)
        {
            foreach (var person in people)
                city[person.Y, person.X] = person.Symbol;
        }


        //Counters for certain events
        static void Counters()
        {
            Console.SetCursorPosition(0, cityHeight + 1);
            Console.WriteLine($"Antal rånade medborgare: {medborgareRobbed}");
            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(0, cityHeight + 2);
            Console.WriteLine($"Antal gripna tjuvar: {tjuvCaught}");
            Console.Write(new string(' ', Console.WindowWidth));
        }


        // Checks if people run into each other and executes CollisionOccurs if they do
        static void CheckCollisions(List<Person> people)
        {
            for (int i = 0; i < people.Count; i++)
            {
                for (int j = i + 1; j < people.Count; j++)
                {
                    if (people[i].X == people[j].X && people[i].Y == people[j].Y)
                    {
                        CollisionOccurs(people[i], people[j]);
                    }
                }
            }
        }


        // Switch for what happens depending on which types of person run into each other
        static void CollisionOccurs(Person person1, Person person2)
        {
            switch (person1, person2)
            {
                case (Tjuv tjuv, Medborgare medborgare):
                    Steal(tjuv, medborgare);
                    break;

                case (Medborgare medborgare, Tjuv tjuv):
                    Steal(tjuv, medborgare);
                    break;

                case (Polis polis, Tjuv tjuv):
                    Confiscate(polis, tjuv);
                    break;

                case (Tjuv tjuv, Polis polis):
                    Confiscate(polis, tjuv);
                    break;
            }
        }


        /* When a Tjuv collides with a Medborgare, 
         * the Tjuv picks a random Sak from Tillhörigheter and transfer it to their Stöldgods */
        static void Steal(Tjuv tjuv, Medborgare medborgare)
        {
            if (medborgare.Saker.Any())
            {
                int randomIndex = rndm.Next(medborgare.Saker.Count);
                Sak stolenSak = medborgare.Saker[randomIndex];

                medborgare.Saker.RemoveAt(randomIndex);
                tjuv.Saker.Add(stolenSak);

                Console.WriteLine($"En tjuv stal {stolenSak.Name} från en medborgare!");
                medborgareRobbed += 1;
                Counters();

                Thread.Sleep(2000);
            }
        }


        /* When a Polis collides with a Tjuv,
         * the Polis transfers everything from Stöldgods to their Beslagtaget */
        static void Confiscate(Polis polis, Tjuv tjuv)
        {
            if (tjuv.Saker.Any())
            {
                var stolenItemsNames = tjuv.Saker.Select(sak => sak.Name).ToList();
                string stolenItemsString = string.Join(", ", stolenItemsNames);

                polis.Saker.AddRange(tjuv.Saker);
                tjuv.Saker.Clear();

                Console.WriteLine($"En polis fångade en tjuv och beslagtog {stolenItemsString}!");
                tjuvCaught += 1;
                Counters();

                Thread.Sleep(2500);
            }
        }
    }
}


/*
static string GenerateName()
{
    string[] Names = { "Nelly", "Ebba", "Wera", "Christin", "Astrid", "Ylva", "Anthony", "Mikael", "Sean", "Nikita", "Arya", "Jakob", "Natan", "Joel", "Noa", "Melker" };
    return Names[rndm.Next(Names.Length)];
}*/