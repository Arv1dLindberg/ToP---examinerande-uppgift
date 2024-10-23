namespace ToP___examinerande_uppgift
{
    internal class Program
    {
        // City Size
        public const int cityWidth = 100;
        public const int cityHeight = 25;

        private static Random rndm = new Random();

        static void Main()
        {
            char[,] city = CreateCity();

            List<Person> people = new List<Person>(); // Creates a list that holds elements of Person
            people.AddRange(PlacePeople<Thief>(30));   // Generates a specified amount of Thieves
            people.AddRange(PlacePeople<Police>(30));   // Generates a specified amount of Police
            people.AddRange(PlacePeople<Citizen>(30)); // Generates a specified amount of Citizens

            //Adds Nycklar, Mobiltelefon, Pengar, and Klocka to citizen's inventory
            foreach (var citizen in people.OfType<Citizen>())
            {
                citizen.Saker.Add(new Sak("Nycklar"));
                citizen.Saker.Add(new Sak("Mobiltelefon"));
                citizen.Saker.Add(new Sak("Pengar"));
                citizen.Saker.Add(new Sak("Klocka"));
            }

            // Repeats the program until manually stopped
            while (true)
            {
                Thread.Sleep(1000);
                ClearPosition(city, people);
                MovePeople(people);
                CheckCollisions(people);
                PeoplePosition(city, people);
                Console.Clear();
                CreateCityWalls(city);
            }
        }

        // Checks if people run into each other
        static void CheckCollisions(List<Person> people)
        {
            for (int i = 0; i < people.Count; i++)
            {
                for (int j = i + 1; j < people.Count; j++)
                {
                    // If two people are at the same coordinates
                    if (people[i].X == people[j].X && people[i].Y == people[j].Y)
                    {
                        CollisionOccurs(people[i], people[j]);
                    }
                }
            }
        }

        // Switch for what happens depending on what type of person's run into each other
        static void CollisionOccurs(Person person1, Person person2)
        {
            switch (person1, person2)
            {
                case (Thief thief, Citizen citizen):
                    Steal(thief, citizen);
                    break;

                case (Citizen citizen, Thief thief):
                    Steal(thief, citizen);
                    break;

                case (Police police, Thief thief):
                    Confiscate(police, thief);
                    break;

                case (Thief thief, Police police):
                    Confiscate(police, thief);
                    break;
            }
        }

        // If a thief collides with a citizen
        static void Steal(Thief thief, Citizen citizen)
        {
            if (citizen.Saker.Any())
            {
                // Pick a random item from the Citizen's belongings
                Random random = new Random();
                int randomIndex = random.Next(citizen.Saker.Count);
                Sak stolenItem = citizen.Saker[randomIndex];

                // Remove the item from the Citizen and add it to the Thief
                citizen.Saker.RemoveAt(randomIndex);
                thief.Saker.Add(stolenItem);

                Console.WriteLine($"A thief stole {stolenItem.Name} from a citizen!");
            }
        }

        // If a police collides with a thief
        static void Confiscate(Police police, Thief thief)
        {
            if (thief.Saker.Any())
            {
                // Removes all items from the Thief's inventory and adds it to the Police's inventory
                police.Saker.AddRange(thief.Saker);
                thief.Saker.Clear();

                Console.WriteLine($"A Police confiscated all items from a thief!");
            }
        }

        // Creates the city boundaries
        static void CreateCityWalls(char[,] city)
        {
            Console.WriteLine(new string('X', cityWidth));
            for (int i = 0; i < cityHeight; i++)
            {
                Console.Write("X");
                for (int j = 1; j < cityWidth - 1; j++)
                    Console.Write(city[i, j]);
                Console.WriteLine("X");
            }
            Console.WriteLine(new string('X', cityWidth));
        }

        // Creates the space that the people will move around in
        static char[,] CreateCity()
        {
            char[,] city = new char[cityHeight, cityWidth];

            for (int i = 0; i < cityHeight; i++)
                for (int j = 0; j < cityWidth; j++)
                    city[i, j] = ' ';
            return city;
        }

        /* Creates and places a specified amount of people within the city and randomly assigns them one of 7 directions
         * (left, right, diagonally left, diagonally right, up, down, and still
         */
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
                // Updates X position
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

                // Updates Y position
                person.Y += person.Ydirection;

                // If a person moves outside the Y walls they will come back on the opposite Y-axis
                if (person.Y < 0)
                {
                    person.Y = Program.cityHeight - 1;
                }
                else if (person.Y >= Program.cityHeight)
                {
                    person.Y = 0;
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
        
        /* List of names that are randomly returned (not used at the moment)
        static string GenerateName()
        {
            string[] Names = { "Anthony", "Mikael", "Sean", "Nikita", "Arya", "Jakob", "Natan", "Joel", "Noa", "Melker", "Nelly", "Ebba", "Wera", "Christin", "Astrid", "Ylva" };
            return Names[rndm.Next(Names.Length)];
        }
        */
    }
}