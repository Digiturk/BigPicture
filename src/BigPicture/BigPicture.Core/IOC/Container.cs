using Autofac;
using Autofac.Configuration;
using BigPicture.Core.Resolver;
using BigPicture.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Core;
using System.Web;
using System.IO;

namespace BigPicture.Core.IOC
{
    public static class Container
    {        
        private static IContainer _Container;

        static Container()
        {
            var path = @"di.json";
            if (HttpContext.Current != null)
            {
                path = HttpContext.Current.Server.MapPath(Path.Combine("~/" + path));
            }

            var config = new ConfigurationBuilder();
            config.AddJsonFile(path);

            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            RegisterResolvers(builder);

            _Container = builder.Build();
        }

        
        private static void RegisterResolvers(ContainerBuilder builder)
        {
            foreach(var resolverDefiniton in ResolversConfig.Instance.Resolvers)
            {//TODO:remove try catch
                try
                {
                    var nodeType = Type.GetType(resolverDefiniton.NodeType);
                    var resolverType = Type.GetType(resolverDefiniton.Resolver);

                    builder
                        .RegisterType(resolverType)
                        .Keyed(resolverDefiniton.Name, typeof(IResolver<>).MakeGenericType(nodeType))
                        .As(typeof(IResolver<>).MakeGenericType(nodeType))
                        .InstancePerDependency();
                }
                catch (Exception ex)
                {

                    throw;
                }
               
            }
        }
     
        public static T Resolve<T>()
        {
            return _Container.Resolve<T>();
        }

        public static object Resolve(Type type)
        {           
            return _Container.Resolve(type);
        }

        public static object TryResolve(Type type)
        {
            object result;
            if(_Container.TryResolve(type, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static object ResolveWithKey(object key, Type type)
        {
            return _Container.ResolveKeyed(key, type);
        }
    }
}
