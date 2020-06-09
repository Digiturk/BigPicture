import { Type } from './type';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Enum extends Type {
  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}