using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.LiskovSubstitution
{
    internal class AnimalController
    {
        public static readonly List<Animal> Animals = new List<Animal>()
        {
            new Pigeon("pigeon")
        };

        public void AnimalLegCount(List<Animal> animals)
        {
            foreach (Animal animal in animals)
            {
                // Here we can provide all derived classes from Animal
                // This method will not know what exactly class substituted
                var legCount = animal.LegCount();
            }
        }
    }
}
