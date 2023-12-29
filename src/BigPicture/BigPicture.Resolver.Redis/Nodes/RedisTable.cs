using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Redis.Nodes
{
    public class RedisTable : Node
    {
        public int db { get; set; }

        public string Name { get; set; }
        public string DatabaseName { get; set; }
    }
}
