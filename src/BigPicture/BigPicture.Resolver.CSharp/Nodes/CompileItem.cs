using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class CompileItem : Node
    {
        public String Path { get; set; }
        public String AbsolutePath { get; set; }
        public String SubType { get; set; }
        public String AutoGen { get; set; }
        public String DesignTime { get; set; }
        public String CodeBlockId { get; set; }
    }
}
