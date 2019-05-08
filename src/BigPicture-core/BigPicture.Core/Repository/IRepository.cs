using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Repository
{
    public interface IRepository
    {
        string TestConnection();        

        string CreateRelationship(String from, String to, String relationShip);

        string CreateNode(String nodeType, object data);

        List<INode> GetAllNodes(String nodeType, Type type);
    }
}