import Node from '../../base/node';
import { Modifier } from '../common';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Field extends Node {
  Assembly: string;
  NameSpace: string;
  OwnerName: string;
  Modifier: Modifier;

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}