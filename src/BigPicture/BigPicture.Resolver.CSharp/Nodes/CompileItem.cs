using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class CompileItem : INode
    {
        public String Id { get; set; }

        public String Name { get; set; }
        public String SubType { get; set; }
        public String AutoGen { get; set; }
        public String DesignTime { get; set; }
    }
}
