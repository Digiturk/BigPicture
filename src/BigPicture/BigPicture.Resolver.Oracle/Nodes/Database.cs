using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Oracle.Nodes
{
    public class Database : INode
    {
        public String Id { get; set; }

        public String Name { get; set; }
        public String Type { get; set; }
        public String[] DoNotParse { get; set; }
    }
}
