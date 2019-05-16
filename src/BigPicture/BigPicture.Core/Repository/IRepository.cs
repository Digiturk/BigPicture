using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Repository
{
    public interface IRepository
    {
        string TestConnection();

        void DeleteAll();

        string CreateRelationship(String from, String to, String relationShip);

        string CreateNode(object node, params String[] nodeTypes);

        void UpdateNode<T>(T node, params String[] nodeTypes) where T : BigPicture.Core.INode;

        List<INode> GetAllNodes(String nodeType, Type type, object filterObject = null);

        List<T> GetAllNodes<T>(String nodeType, object filterObject = null) where T : BigPicture.Core.INode;

        String FindIdOrCreate(object node, String nodeType, object filterObject);
    }
}