import Node from '../base/node';
import * as CSharp from '../c-sharp';
import * as Oracle from '../oracle';

export type Modifier = 'public' | 'private' | 'protected' | 'internal';

//  TODO make this function generic
export const getNodeImplementation = (node: Node): Node => {

  let result: Node;

  if (node.labels.includes("Assembly")) {
    result = new CSharp.Assembly();
  }
  else if (node.labels.includes("Accessor")) {
    result = new CSharp.Accessor();        
  }
  else if (node.labels.includes("Class")) {
    result = new CSharp.Class();            
  }
  else if (node.labels.includes("CompileItem")) {
    result = new CSharp.CompileItem();        
  }  
  else if (node.labels.includes("Constructor")) {
    result = new CSharp.Constructor();        
  }
  else if (node.labels.includes("Destructor")) {
    result = new CSharp.Destructor();        
  }
  else if (node.labels.includes("EnumMember")) {
    result = new CSharp.EnumMember();        
  }
  else if (node.labels.includes("Enum")) {
    result = new CSharp.Enum();        
  }
  else if (node.labels.includes("Field")) {
    result = new CSharp.Field();        
  }
  else if (node.labels.includes("Interface")) {
    result = new CSharp.Interface();        
  }
  else if (node.labels.includes("Method")) {
    result = new CSharp.Method();        
  }
  else if (node.labels.includes("NameSpace")) {
    result = new CSharp.NameSpace();        
  }
  else if (node.labels.includes("Parameter")) {
    result = new CSharp.Parameter();        
  }
  else if (node.labels.includes("Project")) {
    result = new CSharp.Project();        
  }
  else if (node.labels.includes("Property")) {
    result = new CSharp.Property();        
  }
  else if (node.labels.includes("Solution")) {
    result = new CSharp.Solution();        
  }
  else if (node.labels.includes("Struct")) {
    result = new CSharp.Struct();        
  }
  else if (node.labels.includes("Column")) {
    result = new Oracle.Column();        
  }
  else if (node.labels.includes("Database")) {
    result = new Oracle.Database();        
  }
  else if (node.labels.includes("DbObject")) {
    result = new Oracle.DbObject();        
  }
  else if (node.labels.includes("Schema")) {
    result = new Oracle.Schema();        
  }
  else {
    result = new Node();
  }

  Object.assign(result, node);
  
  return result;
}