using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.DependencyInversion
{
    internal class XmlHttpService : IConnection
    {
        public object Request(string uri)
        {
            // Return Xml request
            return null;
        }
    }
}
