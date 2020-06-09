import Node from '../../base/node';
import { Modifier } from '../common';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Accessor extends Node {
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