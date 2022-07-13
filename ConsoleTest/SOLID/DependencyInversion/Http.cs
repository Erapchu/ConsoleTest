using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SOLID.DependencyInversion
{
    internal class Http
    {
        private readonly IConnection _connection;

        public Http(IConnection connection)
        {
            // Here we going again with Liskov substitution!
            // We don't have dependencies to other implementations of this interface (they may be in different .dll)
            // High-level module have no dependencies to Http Services
            // Abstractions should not depend on details. Details should depend on abstractions.
            _connection = connection;
        }

        public object Get(string uri)
        {
            // Also, with DI here we don't know exactly what class perform request
            return _connection.Request(uri);
        }
    }

    internal class WrongHttp
    {
        private readonly XmlHttpService _xmlHttpService;

        public WrongHttp(XmlHttpService xmlHttpService)
        {
            _xmlHttpService = xmlHttpService;
        }
    }
}
