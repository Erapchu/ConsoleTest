using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.LiskovSubstitution
{
    internal class Animal
    {
        public string Name { get; set; }

        public Animal(string name)
        {

        }

        public virtual int LegCount() { return 0; }
    }
}
