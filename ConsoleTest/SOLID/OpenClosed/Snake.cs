using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.OpenClosed
{
    internal class Snake : Animal
    {
        public Snake(string name) : base(name)
        {

        }

        public override void MakeSound()
        {
            // return "hiss";
        }
    }
}
