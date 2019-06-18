using BigPicture.Core.Resolver;
using BigPicture.Resolver.CSharp.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp
{
    public class ExecuteNonQueryResolver : IResolver<DbCallBlock>
    {
        public void Resolve(DbCallBlock obj)
        {            
            throw new NotImplementedException();
        }
    }
}
