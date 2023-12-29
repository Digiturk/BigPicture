using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Redis.Nodes
{
    public class Hash : Node
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Table { get; set; }
    }
}
