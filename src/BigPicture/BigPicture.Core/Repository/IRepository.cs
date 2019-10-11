using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Repository
{
    public interface IRepository
    {
        string TestConnection();

        void DeleteAll();

        T FindNode<T>(String id) where T : BigPicture.Core.Node;

        T FindNode<T>(object filterObject, params String[] nodeTypes) where T : BigPicture.Core.Node;

        string CreateRelationship(String from, String to, String relationShip, dynamic dataObject = null);

        string CreateNode(object node, params String[] nodeTypes);

        void UpdateNode<T>(T node, params String[] nodeTypes) where T : BigPicture.Core.Node;

        List<Node> GetAllNodes(String nodeType, Type type, object filterObject = null);

        List<T> GetAllNodes<T>(String nodeType, object filterObject = null) where T : BigPicture.Core.Node;

        String FindIdOrCreateSubNode(object node, String nodeType, String id, String relation, String subNodeType, dynamic filterObject = null);

        String FindIdOrCreate(object node, String nodeType, object filterObject);

        String FindIdOrCreate(object node, String[] nodeTypes, object filterObject);

        List<Entity> RunCustomQuery(String query, Type type);
    }
}