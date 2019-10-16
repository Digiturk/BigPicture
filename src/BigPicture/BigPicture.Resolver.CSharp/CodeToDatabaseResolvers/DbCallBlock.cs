using BigPicture.Core;
using BigPicture.Resolver.CSharp.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp
{
    public class DbCallBlock : Entity
    {
        public String Id { get; set; }

        public Method Code { get; set; }
        public Access Calls { get; set; }
        public Method DbObject { get; set; }        
    }
}
