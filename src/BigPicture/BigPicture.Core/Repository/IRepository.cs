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

        string CreateNode(String nodeType, object data);

        void UpdateNode<T>(T node) where T : BigPicture.Core.INode;

        List<INode> GetAllNodes(String nodeType, Type type, dynamic filterObject = null);

        List<T> GetAllNodes<T>(String nodeType, dynamic filterObject = null) where T : BigPicture.Core.INode;
    }
}