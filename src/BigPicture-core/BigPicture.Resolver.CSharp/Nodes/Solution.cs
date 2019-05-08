using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Solution : INode
    {
        public String Id { get; set; }
        public String Path { get; set; }
        public String Name { get; set; }
    }
}
