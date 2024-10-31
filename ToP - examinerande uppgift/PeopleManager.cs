using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToP___examinerande_uppgift
{
    internal class PeopleManager
    {
        private static Random rndm = new Random();

        // Generates a specified amount of people
        public static List<Person> AddPeople()
        {
            List<Person> people = new List<Person>();
            people.AddRange(PlacePeople<Tjuv>(10));
            people.AddRange(PlacePeople<Polis>(15));
            people.AddRange(PlacePeople<Medborgare>(30));
            return people;
        }

        // Adds items to Tillhörigheter
        public static void AddToTillhörigheter(List<Person> people)
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
        public static List<T> PlacePeople<T>(int peopleCount) where T : Person, new()
        {
            List<T> people = new List<T>();
            for (int i = 0; i < peopleCount; i++)
            {
                int x = rndm.Next(1, CityManager.GetCityWidth() - 1);
                int y = rndm.Next(1, CityManager.GetCityHeight() - 1);

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

        // Gives the person's their direction and moves them. If they move outside the city walls they will come back on the other side.
        public static void MovePeople(List<Person> people)
        {
            foreach (var person in people)
            {
                person.X += person.Xdirection;

                if (person.X <= 0)
                {
                    person.X = CityManager.GetCityWidth() - 1;
                }
                else if (person.X >= CityManager.GetCityWidth())
                {
                    person.X = 1;
                }

                person.Y += person.Ydirection;

                if (person.Y <= 0)
                {
                    person.Y = CityManager.GetCityHeight() - 1;
                }
                else if (person.Y >= CityManager.GetCityHeight())
                {
                    person.Y = 1;
                }
            }
        }

        // Clears the person's previous position
        public static void ClearPosition(char[,] city, List<Person> people)
        {
            foreach (var person in people)
                city[person.Y, person.X] = ' ';
        }

        // Updates person's position in the city
        public static void UpdatePeoplePosition(char[,] city, List<Person> people)
        {
            foreach (var person in people)
                city[person.Y, person.X] = person.Symbol;
        }
    }
}
