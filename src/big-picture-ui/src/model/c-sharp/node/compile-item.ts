import Node from '../../base/node';
import { CodeEditor } from '../../components/code-editor';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

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
      NavigationItems.code
    ];
  }
}