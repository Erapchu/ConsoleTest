using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.InterfaceSegregation
{
    internal interface ICircle
    {
        void DrawCircle();
    }

    internal class Circle : ICircle
    {
        // Draw only cirlce
        public void DrawCircle()
        {

        }
    }
}
