import { Type } from './type';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Interface extends Type {

  public getShortLabel (): string { 
    return "In";
  }

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}