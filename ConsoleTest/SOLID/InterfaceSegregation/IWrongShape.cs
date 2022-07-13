using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.InterfaceSegregation
{
    internal interface IWrongShape
    {
        void Draw();
        void DrawCircle();
    }

    // That's wrong, we shouldn't implement Draw methods for all shapes inside single interface
    internal class WrongShape : IWrongShape
    {
        public void Draw()
        {

        }

        public void DrawCircle()
        {

        }
    }
}
