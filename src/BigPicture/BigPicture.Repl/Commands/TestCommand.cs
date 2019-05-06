using BigPicture.Core.Repository;
using BigPicture.Core.Resolver;
using BigPicture.IOC;
using BigPicture.Resolver.CSharp.Nodes;
using Replify.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Repl.Commands
{
    [Command("test")]
    public class TestCommand : BaseCommand
    {
        public override void Run(string param = "")
        {
            if (param == "repo")
            {
                var repo = Container.Resolve<IRepository>();
                Console.WriteLine(repo.TestConnection());
            }
            else
            {
                Console.Error.WriteLine("Unknown parameter. Test parameter should be one of these values [repo]");
            }
        }
    }
}
