using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Core.Repository
{
    public interface IGraphRepository
    {
        Node GetNodeById(String id);
        IEnumerable<Node> FindNodesByName(String name, int limit = 5, int skip = 0);
    }
}
