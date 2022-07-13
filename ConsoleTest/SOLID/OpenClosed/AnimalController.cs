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
                // Each derived class (from Animal) is opened for extention
                // We will not change Animal class directly, but create subclasses and override behavior
                animal.MakeSound();
            }
        }
    }
}
