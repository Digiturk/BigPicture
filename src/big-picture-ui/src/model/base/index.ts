import Node from '../base/node';
import * as CSharp from '../c-sharp';
import * as Oracle from '../oracle';

export type Modifier = 'public' | 'private' | 'protected' | 'internal';

//  TODO make this function generic
export const getNodeImplementation = (node: Node): Node => {

  let result: Node;

  if (node.Labels.includes("Assembly")) {
    result = new CSharp.Assembly();
  }
  else if (node.Labels.includes("Accessor")) {
    result = new CSharp.Accessor();        
  }
  else if (node.Labels.includes("Class")) {
    result = new CSharp.Class();            
  }
  else if (node.Labels.includes("CompileItem")) {
    result = new CSharp.CompileItem();        
  }  
  else if (node.Labels.includes("Constructor")) {
    result = new CSharp.Constructor();        
  }
  else if (node.Labels.includes("Destructor")) {
    result = new CSharp.Destructor();        
  }
  else if (node.Labels.includes("EnumMember")) {
    result = new CSharp.EnumMember();        
  }
  else if (node.Labels.includes("Enum")) {
    result = new CSharp.Enum();        
  }
  else if (node.Labels.includes("Field")) {
    result = new CSharp.Field();        
  }
  else if (node.Labels.includes("Interface")) {
    result = new CSharp.Interface();        
  }
  else if (node.Labels.includes("Method")) {
    result = new CSharp.Method();        
  }
  else if (node.Labels.includes("NameSpace")) {
    result = new CSharp.NameSpace();        
  }
  else if (node.Labels.includes("Parameter")) {
    result = new CSharp.Parameter();        
  }
  else if (node.Labels.includes("Project")) {
    result = new CSharp.Project();        
  }
  else if (node.Labels.includes("Property")) {
    result = new CSharp.Property();        
  }
  else if (node.Labels.includes("Solution")) {
    result = new CSharp.Solution();        
  }
  else if (node.Labels.includes("Struct")) {
    result = new CSharp.Struct();        
  }
  else if (node.Labels.includes("Column")) {
    result = new Oracle.Column();        
  }
  else if (node.Labels.includes("Database")) {
    result = new Oracle.Database();        
  }
  else if (node.Labels.includes("DbObject")) {
    result = new Oracle.DbObject();        
  }
  else if (node.Labels.includes("Schema")) {
    result = new Oracle.Schema();        
  }
  else {
    result = new Node();
  }

  Object.assign(result, node);
  
  return result;
}