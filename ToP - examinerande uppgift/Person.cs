using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToP___examinerande_uppgift
{
    internal class Person
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Xdirection { get; set; }
        public int Ydirection { get; set; }
        public List<Sak> Saker { get; set; } = new List<Sak>();

        public char Symbol => GetSymbol();
        protected virtual char GetSymbol() => ' ';
        public virtual string InventoryName => " ";

    }

    // Represents items people have in their inventory
    internal class Sak
    {
        public string Name { get; set; }

        public Sak(string name)
        {
            Name = name;
        }
    }


    // Inheritances of Person
    class Tjuv : Person 
    {
        protected override char GetSymbol() => 'T';
        public override string InventoryName => "Stöldgods";
    }

    class Polis : Person
    {
        protected override char GetSymbol() => 'P';
        public override string InventoryName => "Beslagtaget";
    }

    class Medborgare : Person
    {
        protected override char GetSymbol() => 'M';
        public override string InventoryName => "Tillhörigheter";
    }
}
