using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlClient
{
    class Setup
    {
        public static Settings Run()
        {
            Settings settings = new Settings();
            Console.WriteLine(string.Format("Connection url? (default: {0})", settings.url));
            string url = Console.ReadLine();
            if (url != "")
            {
                settings.url = url;
            }

            return settings;
        }
    }
}
