using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Project : Node
    {
        public String AbsolutePath { get; set; }
        public String ProjectGuid { get; set; }
        public String RelativePath { get; set; }

        public String OutputType { get; set; }
        public String AssemblyName { get; set; }
        public String TargetFrameworkVersion { get; set; }
    }
}
