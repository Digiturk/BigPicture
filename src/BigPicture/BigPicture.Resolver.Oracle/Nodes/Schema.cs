using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Oracle.Nodes
{
    public class Schema : INode
    {
        public String Id { get; set; }

        public String DatabaseName { get; set; }
        public String Name { get; set; }
    }
}
