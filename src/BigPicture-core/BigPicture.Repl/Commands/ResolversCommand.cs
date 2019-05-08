using BigPicture.Core;
using BigPicture.Core.Resolver;
using BigPicture.Core.IOC;
using BigPicture.Core.Config;
using Replify.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Repl.Commands
{
    [Command("resolvers")]
    public class ResolversCommand : BaseCommand
    {        
        public override void Run(string param = "")
        {            
            foreach(var resolver in ResolversConfig.Instance.Resolvers)
            {
                Console.WriteLine(resolver.Name + " (" + resolver.Resolver + ")");
            }
        }
    }
}
