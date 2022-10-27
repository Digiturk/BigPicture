import Node from "../base/node";
import * as CSharp from "../c-sharp";
import * as Oracle from "../oracle";

export type Modifier = "public" | "private" | "protected" | "internal";

//  TODO make this function generic
export const getNodeImplementation = (node: Node): Node => {
  let result: Node;

  const nodeMap = {
    Assembly: CSharp.Assembly,
    Accessor: CSharp.Accessor,
    Class: CSharp.Class,
    CompileItem: CSharp.CompileItem,
    Constructor: CSharp.Constructor,
    Destructor: CSharp.Destructor,
    EnumMember: CSharp.EnumMember,
    Enum: CSharp.Enum,
    Field: CSharp.Field,
    Interface: CSharp.Interface,
    Method: CSharp.Method,
    NameSpace: CSharp.NameSpace,
    Parameter: CSharp.Parameter,
    Project: CSharp.Project,
    Property: CSharp.Property,
    Solution: CSharp.Solution,
    Struct: CSharp.Struct,
    Column: Oracle.Column,
    Database: Oracle.Database,
    DbObject: Oracle.DbObject,
    Schema: Oracle.Schema,
  };

  type Keys = keyof typeof nodeMap;
  type NodeTypes = typeof nodeMap[Keys];
  type ExtractInstanceType<T> = T extends new () => infer R ? R : never;

  class NodeFactory {
    static getNode(k: string): ExtractInstanceType<NodeTypes> {
      if (k in nodeMap) {
        return new nodeMap[k]();
      }
      return new Node();
    }
  }

  class NodeBaseService {
    static getNodeByKey(node: string) {
      return NodeFactory.getNode(node);
    }
  }

  result = NodeBaseService.getNodeByKey(node.Labels[0]);

  Object.assign(result, node);

  return result;
};
