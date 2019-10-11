using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Accessor : Node
    {
        public String Name { get; set; }
        public String Modifier { get; set; }
        public bool HasBody { get; set; }
    }
}
