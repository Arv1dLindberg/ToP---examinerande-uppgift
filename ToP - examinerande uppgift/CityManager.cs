using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToP___examinerande_uppgift
{
    internal class CityManager
    {
        // City Size
        private const int cityWidth = 100;
        private const int cityHeight = 25;
        public static int GetCityWidth() => cityWidth;
        public static int GetCityHeight() => cityHeight;


        // Creates X's that serve as boundaries of the city
        public static void CreateCityWalls(char[,] city)
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
        public static char[,] CreateInnerCity()
        {
            char[,] city = new char[cityHeight, cityWidth];

            for (int i = 1; i < cityHeight - 1; i++)
                for (int j = 1; j < cityWidth - 1; j++)
                    city[i, j] = ' ';
            return city;
        }
    }
}

