using BigPicture.Repl.Commands;
using Replify.Net.Commands;
using System;
using System.Reflection;

namespace BigPicture.Repl
{
    class Program
    {
        static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("BigPicture Repl, version " + version);

            Replify.Net.Replify.RegisterCommand<ExitCommand>();
            Replify.Net.Replify.RegisterCommand<Test>();

            Replify.Net.Replify.Start();
        }
    }
}
