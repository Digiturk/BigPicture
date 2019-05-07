using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Project : INode
    {
        public String Id { get; set; }
        public String AbsolutePath { get; set; }
        public String ProjectGuid { get; set; }
        public String RelativePath { get; set; }
        public String Name { get; set; }
    }
}
