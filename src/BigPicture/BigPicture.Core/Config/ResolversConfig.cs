﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigPicture.Core.Config
{
    public class ResolversConfig
    {
        public static ResolversConfig _Instance = null;
        public static ResolversConfig Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = JsonConvert.DeserializeObject<ResolversConfig>(File.ReadAllText(@"di-resolvers.json"));
                }

                return _Instance;
            }
        }

        public Options Options { get; set; }
        public List<ResolverDefinition> Resolvers { get; set; }
        public List<StartData> StartData { get; set; }
    }

    public class Options
    {
        public bool RemoveAllOnStart { get; set; }
    }

    public class ResolverDefinition
    {
        public String Name { get; set; }
        public String Resolves { get; set; }
        public bool RunParallel { get; set; }
        public Int32? MaxParallel { get; set; }
        public String NodeType { get; set; }
        public String Resolver { get; set; }
        public String CustomQuery { get; set; }
    }

    public class StartData
    {
        public String NodeTypeName { get; set; }
        public object Data { get; set; }
    }
}
