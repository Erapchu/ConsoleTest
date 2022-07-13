using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.InterfaceSegregation
{
    internal interface IShape
    {
        void Draw();
    }

    internal class CustomeShape : IShape
    {
        // Universal interface to draw some shape
        public void Draw()
        {

        }
    }
}
