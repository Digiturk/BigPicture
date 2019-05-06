using Autofac;
using Autofac.Configuration;
using BigPicture.Core.Resolver;
using BigPicture.Core.Config;
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

            RegisterResolvers(builder);

            _Container = builder.Build();
        }

        
        private static void RegisterResolvers(ContainerBuilder builder)
        {
            foreach(var resolverDefiniton in ResolversConfig.Instance.Resolvers)
            {
                // TODO register resolver to container

                var nodeType = Type.GetType(resolverDefiniton.NodeType);
                var resolverType = Type.GetType(resolverDefiniton.Resolver);

                builder.RegisterType(resolverType).As(typeof(IResolver<>).MakeGenericType(nodeType)).InstancePerDependency();
            }
        }

        public static T Resolve<T>()
        {
            return _Container.Resolve<T>();
        }
    }
}
