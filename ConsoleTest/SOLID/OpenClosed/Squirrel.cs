using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.OpenClosed
{
    internal class Squirrel : Animal
    {
        public Squirrel(string name) : base(name)
        {

        }

        public override void MakeSound()
        {
            // return "squeak";
        }
    }
}
