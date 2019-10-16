using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Oracle.Nodes
{
    public class Database : Node
    {
        public String Type { get; set; }
        public String[] DoNotParse { get; set; }
    }
}
