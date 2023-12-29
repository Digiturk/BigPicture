using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Redis.Nodes
{
    public class CacheServer : Node
    {
        public String Type { get; set; }
        public String[] DoNotParse { get; set; }
    }
}
