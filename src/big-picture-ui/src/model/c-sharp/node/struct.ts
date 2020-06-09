import { Type } from './type';
import NavigationItem, { NavigationItems } from '../../components/base/navigation-item';

export class Struct extends Type {
  IsAbstract: boolean;
  IsSealed: boolean;
  IsStatic: boolean;

  public getNavigationItems(): NavigationItem[]  {
    var superNavItems = super.getNavigationItems();
    return [
      ...superNavItems,
      NavigationItems.code
    ];
  }
}