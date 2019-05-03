﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigPicture.IOC.Config
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

        public List<ResolverDefinition> Resolvers { get; set; }
    }

    public class ResolverDefinition
    {
        public String Name { get; set; }
        public String NodeType { get; set; }
        public String Resolver { get; set; }
    }
}