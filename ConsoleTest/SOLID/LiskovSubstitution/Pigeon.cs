using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.LiskovSubstitution
{
    internal class Pigeon : Animal
    {
        public Pigeon(string name) : base(name)
        {

        }

        public override int LegCount()
        {
            return 4;
        }
    }
}
