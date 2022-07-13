using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.SingleResponsibility
{
    /// <summary>
    /// Incorrect Animal class
    /// </summary>
    internal class WrongAnimal
    {
        public string Name { get; set; }

        public WrongAnimal(string name)
        {

        }

        public void SaveAnimal()
        {
            
        }
    }

    /// <summary>
    /// Correct Animal class according to Single Resbonsibility principle
    /// </summary>
    internal class Animal
    {
        public string Name { get; set; }

        public Animal(string name)
        {

        }
    }

    internal class AnimalDB
    {
        public Animal GetAnimal()
        {
            // Get animal from source
            return null;
        }

        public void SaveAnimal(Animal animal)
        {
            // Save animal to source
        }
    }
}
