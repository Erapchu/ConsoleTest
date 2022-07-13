using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.OpenClosed
{
    internal class Animal
    {
        public string Name { get; set; }

        public Animal(string name)
        {

        }

        public virtual void MakeSound()
        {
            // return "arrarrarggh - some sound"
        }
    }
}
