using BigPicture.Core;
using BigPicture.Core.Resolver;
using BigPicture.Core.IOC;
using Replify.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Repl.Commands
{
    [Command("start")]
    public class StartCommand : BaseCommand
    {
        [Parameter("load-start-data", ShortKey = "l")]
        public String LoadStartData { get; set; } = null;

        public override void Run(string param = "")
        {
            var resolveEngine = Container.Resolve<IResolveEngine>();

            if(LoadStartData != null)
            {
                resolveEngine.LoadStartData();
                return;
            }

            if (String.IsNullOrEmpty(param))
            {
                resolveEngine.StartResolvers();
            }
            else
            {
                resolveEngine.StartResolver(param);
            }
        }
    }
}
