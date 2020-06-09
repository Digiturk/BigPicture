import Node from '../../base/node';
import { Modifier } from '../common';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Constructor extends Node {
  Modifier: Modifier;

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}