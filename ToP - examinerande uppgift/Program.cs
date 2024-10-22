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
            people.AddRange(PlacePeople<Thief>(10));   // Generates a specified amount of Thieves
            people.AddRange(PlacePeople<Police>(5));   // Generates a specified amount of Police
            people.AddRange(PlacePeople<Citizen>(30)); // Generates a specified amount of Citizens

            // Repeats the program until manually stopped
            while (true)
            {
                Thread.Sleep(1000);
                ClearPosition(city, people);
                MovePeople(people);
                PeoplePosition(city, people);
                Console.Clear();
                CreateCityWalls(city);
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
        
        // List of names that are randomly returned
        static string GenerateName()
        {
            string[] Names = { "Anthony", "Mikael", "Sean", "Nikita", "Arya", "Jakob", "Natan", "Joel", "Noa", "Melker", "Nelly", "Ebba", "Wera", "Christin", "Astrid", "Ylva" };
            return Names[rndm.Next(Names.Length)];
        }
    }
}