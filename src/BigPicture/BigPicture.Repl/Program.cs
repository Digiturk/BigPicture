using BigPicture.Repl.Commands;
using BigPicture.Resolver.CSharp.Resolvers;
using Replify.Net.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Repl
{
    class Program
    {
        static void Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("BigPicture Repl, version " + version);

            Replify.Net.Replify.Register<ExitCommand>();
            Replify.Net.Replify.Register<TestCommand>();
            Replify.Net.Replify.Register<StartCommand>();
            Replify.Net.Replify.Register<ResolversCommand>();

            Replify.Net.Replify.Start();
        }
    }
}
