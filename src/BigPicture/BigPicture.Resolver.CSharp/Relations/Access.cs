using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Access : Relation
    {
        public String Id { get; set; }
        public string Name { get { return "ACCESS"; } }

        public String Code { get; set; }       
        public List<String> ParamNames { get; set; }
        public List<String> ParamValues { get; set; }
        public List<String> ParamCodes { get; set; }
    }
}
