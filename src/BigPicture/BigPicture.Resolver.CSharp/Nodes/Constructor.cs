using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.Nodes
{
    class Constructor : INode
    {
        public String Id { get; set; }

        public String Name { get; set; }
        public String Modifier { get; set; }
    }
}
