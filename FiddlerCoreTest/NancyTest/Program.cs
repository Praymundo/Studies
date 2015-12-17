using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyTest
{
    class Program
    {
        static Nancy.Hosting.Self.NancyHost _Host;
        static void Main(string[] args)
        {
            // initialize an instance of NancyHost (found in the Nancy.Hosting.Self package)
            _Host = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:56772"));
            _Host.Start(); // start hosting
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            string str = "";
            while (str != "q")
            {
                str = Console.ReadLine();
            }
            _Host.Stop();

        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _Host.Stop();
        }
    }

    public class Bootstrapper : Nancy.DefaultNancyBootstrapper
    {
        protected virtual Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return Nancy.Bootstrapper.NancyInternalConfiguration.Default;
            }
        }
    }

}
