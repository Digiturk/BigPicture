import { Type } from './type';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class EnumMember extends Type {
  Value: string;

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}