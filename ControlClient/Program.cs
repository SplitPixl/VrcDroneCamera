using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("VRC Drone Cam 2.0 - Control Client");
            Settings settings = Setup.Run();
            Client client = new Client(settings.url);
            client.Connect();
        }
    }
}
