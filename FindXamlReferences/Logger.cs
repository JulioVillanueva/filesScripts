using FindXamlReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindXamlReferences
{
    public class Logger : ILogger
    {
        public Logger() { }

        public void Log(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss > ") + msg);
        }
    }
}
