using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToP___examinerande_uppgift
{
    internal class Person
    {
        public int X { get; set; } // X coordinate
        public int Y { get; set; } // Y coordinate
        public int Xdirection { get; set; } // Direction along the X-axis
        public int Ydirection { get; set; } // Direction along the Y-axis
        public string Name { get; set; }
        public char Symbol => GetSymbol(); // Symbol representing people
        protected virtual char GetSymbol() => ' ';

        
    }
    // Arv av Person
    class Thief : Person 
    {
        protected override char GetSymbol() => 'T';
    }

    class Police : Person
    {
        protected override char GetSymbol() => 'P';
    }

    class Citizen : Person
    {
        protected override char GetSymbol() => 'M';
    }
}
