using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Struct : Nodes.Type
    {
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public bool IsStatic { get; set; }
    }
}
