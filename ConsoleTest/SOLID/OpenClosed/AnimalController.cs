using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.OpenClosed
{
    internal class AnimalController
    {
        public static readonly List<Animal> Animals = new List<Animal>()
        {
            new Lion("lion"),
            new Snake("mouse"),
            new Squirrel("squirrel")
        };

        public void AnimalSound(List<Animal> animals)
        {
            foreach (Animal animal in animals)
            {
                animal.MakeSound();
            }
        }
    }
}
