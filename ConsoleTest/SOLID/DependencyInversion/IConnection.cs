using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.DependencyInversion
{
    internal interface IConnection
    {
        object Request(string uri);
    }
}
