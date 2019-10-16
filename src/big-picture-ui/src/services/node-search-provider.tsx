import * as React from 'react';
import * as WFace from '@wface/components';
import { ISearchProvider, IMenuTreeItem, IHttpService, MenuTreeUtil } from '@wface/ioc';
import { injectable, inject } from "inversify";
import Node from '../model/base/node';
import { getNodeImplementation } from '../model/base';
import { AppContext } from '@wface/store';


@injectable()
export default class NodeSearchProvider implements ISearchProvider {

  @inject("openScreen") openScreen: (item: IMenuTreeItem, initialValues?: any) => void;
  @inject("IHttpService") httpService: IHttpService;
  @inject("AppContext") appContext: AppContext;
  nodeBrowserScreen: IMenuTreeItem;


  search(term: string): Promise<Node[]> {
    return new Promise<Node[]>((resolve, reject) => {

      this.httpService.get<Node[]>("node/search/" + term + "/8")
        .then(resolve)
        .catch(error => {
          console.error(error);
        });
      // TODO reject
    });
  }

  renderSearchItem(node: Node): React.ReactNode {
    const implementation = getNodeImplementation(node);
    return (
      <WFace.WListItem id={"search-item-" + node.Id} dense key={"key-search-item-" + node.Id}>
        <WFace.WListItemIcon>
          {implementation.getIcon()}
        </WFace.WListItemIcon>
        <WFace.WListItemText primary={node.Name} secondary={node.Labels.join(', ')} />
      </WFace.WListItem>
    );
  }

  onItemSelected(node: Node) {
    if (!this.nodeBrowserScreen) {
      this.nodeBrowserScreen = MenuTreeUtil.findByName(this.appContext.menuTree, "NodeBrowser");
    }

    if (this.nodeBrowserScreen) {
      const implementation = getNodeImplementation(node);
      this.openScreen(this.nodeBrowserScreen, { node: implementation });
    }
  }
}