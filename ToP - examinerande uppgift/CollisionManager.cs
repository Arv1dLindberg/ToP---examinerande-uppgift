using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToP___examinerande_uppgift
{
    internal class CollisionManager
    {
        // Checks if people run into each other and executes CollisionOccurs(); if they do
        public static void CheckCollisions(List<Person> people, ref int medborgareRobbed, ref int tjuvCaught)
        {
            for (int i = 0; i < people.Count; i++)
            {
                for (int j = i + 1; j < people.Count; j++)
                {
                    if (people[i].X == people[j].X && people[i].Y == people[j].Y)
                    {
                        CollisionOccurs(people[i], people[j], ref medborgareRobbed, ref tjuvCaught);
                    }
                }
            }
        }

        // Switch for what happens depending on which types of person's run into each other
        private static void CollisionOccurs(Person person1, Person person2, ref int medborgareRobbed, ref int tjuvCaught)
        {
            switch (person1, person2)
            {
                case (Tjuv tjuv, Medborgare medborgare):
                    Steal(tjuv, medborgare, ref medborgareRobbed);
                    break;

                case (Medborgare medborgare, Tjuv tjuv):
                    Steal(tjuv, medborgare, ref medborgareRobbed);
                    break;

                case (Polis polis, Tjuv tjuv):
                    Confiscate(polis, tjuv, ref tjuvCaught);
                    break;

                case (Tjuv tjuv, Polis polis):
                    Confiscate(polis, tjuv, ref tjuvCaught);
                    break;
            }
        }

        /* When a Tjuv collides with a Medborgare, 
         * the Tjuv picks a random Sak from Tillhörigheter and transfer it to their Stöldgods */
        private static void Steal(Tjuv tjuv, Medborgare medborgare, ref int medborgareRobbed)
        {
            if (medborgare.Saker.Any())
            {
                int randomIndex = new Random().Next(medborgare.Saker.Count);
                Sak stolenSak = medborgare.Saker[randomIndex];

                medborgare.Saker.RemoveAt(randomIndex);
                tjuv.Saker.Add(stolenSak);
                medborgareRobbed += 1;

                Program.printCounters();
                Console.WriteLine($"En tjuv stal {stolenSak.Name} från en medborgare!");

                Thread.Sleep(2000);
            }
        }

        /* When a Polis collides with a Tjuv,
         * the Polis transfers everything from Stöldgods to their Beslagtaget */
        private static void Confiscate(Polis polis, Tjuv tjuv, ref int tjuvCaught)
        {
            if (tjuv.Saker.Any())
            {
                var stolenItemsNames = tjuv.Saker.Select(sak => sak.Name).ToList();
                string stolenItems = string.Join(", ", stolenItemsNames);

                polis.Saker.AddRange(tjuv.Saker);
                tjuv.Saker.Clear();
                tjuvCaught += 1;

                Program.printCounters();
                Console.WriteLine($"En polis fångade en tjuv och beslagtog {stolenItems}!");

                Thread.Sleep(2500);
            }
        }
    }
}
