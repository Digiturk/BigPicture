import Node from '../../base/node';
import { CodeEditor } from '../../components/code-editor';
import NavigationItem from '../../components/base/navigation-item';

export class CompileItem extends Node {
  Path: string;
  AbsolutePath: string;
  SubType: string;
  AutoGen: string;
  DesignTime: string;

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      {
        icon: 'layers',
        text: 'Code',
        component: CodeEditor
      }
    ];
  }
}