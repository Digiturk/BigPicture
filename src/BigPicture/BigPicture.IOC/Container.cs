using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.IOC
{
    public static class Container
    {        
        private static IContainer _Container;

        static Container()
        {                        
            var config = new ConfigurationBuilder();
            config.AddJsonFile("di.json");

            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);            

            _Container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return _Container.Resolve<T>();
        }
    }
}
