import Node from '../../base/node';
import { Modifier } from '../common';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Method extends Node {
  Assembly: string;
  NameSpace: string;
  OwnerName: string;
  Modifier: Modifier;
  HasBody: boolean;

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}