using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Oracle.Nodes
{
    public class DbObject : Node
    {
        public String DatabaseName { get; set; }
        public String SchemaName { get; set; }
        public String Type { get; set; }
    }
}
