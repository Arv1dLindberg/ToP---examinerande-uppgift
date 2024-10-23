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
        public List<Sak> Saker { get; set; } = new List<Sak>(); // Stores Sak instances in a list

        public char Symbol => GetSymbol(); // Symbol representing people
        protected virtual char GetSymbol() => ' '; // Default Symbol
        
        public virtual string InventoryName => " "; // Default InventoryName

    }

    // Adds a Sak along with a constructor
    internal class Sak
    {
        public string Name { get; set; }

        public Sak(string name)
        {
            Name = name;
        }
    }

    // Inheritances of Person
    class Thief : Person 
    {
        protected override char GetSymbol() => 'T'; // Overrides Default Symbol
        public override string InventoryName => "Stöldgods"; // Overrides default InventoryName
    }

    class Police : Person
    {
        protected override char GetSymbol() => 'P'; // Overrides Default Symbol
        public override string InventoryName => "Beslagtaget"; // Overrides default InventoryName
    }

    class Citizen : Person
    {
        protected override char GetSymbol() => 'M'; // Overrides Default Symbol
        public override string InventoryName => "Tillhörigheter"; //Overrides default InventoryName
    }
}
