import * as React from 'react';
import * as WFace from '@wface/components';
import { Profile } from '../../components/node-components/profile';
import LoadingPage from './loading-page';
import Node from '../../model/base/node';
import { getNodeImplementation } from '../../model/base';


interface NodeBrowserState {
  component: React.ReactType<{node?: Node}>;
  node: Node;
  isLoading: boolean;
}

export class NodeBrowser extends React.Component<WFace.BaseScreenProps, NodeBrowserState> {
  constructor(props: WFace.BaseScreenProps) {
    super(props);

    this.state = this.props.screenData.state || {
      component: null,
      node: null,
      isLoading: false,
    }
  }

  componentDidMount() {
    this.loadNode();
  }

  componentDidUpdate(prevProps: WFace.BaseScreenProps, prevState: NodeBrowserState, snapshot) {
    if (prevState.node && prevState.node.Id != this.props.screenData.initialValues.node.Id && !this.state.isLoading) {
      this.loadNode();
    }
  }

  loadNode = () => {
    this.setState({ isLoading: true }, () => {
      this.props.httpService.get('node/' + this.props.screenData.initialValues.node.Id)
        .then(node => {
          const implementation = getNodeImplementation(node);
          this.setState({ node: implementation });
        })
        .catch(error => {
          this.props.showSnackbar("Node data could not be loaded");
          console.error(error);
        })
        .finally(() => this.setState({ isLoading: false }));
    });
  }

  public render() {
    const node = this.state.node || this.props.screenData.initialValues.node;

    return (
      <WFace.WGrid container spacing={2}>
        <WFace.WGrid item xl={2} lg={3} md={5} sm={6} xs={12}>          
          <Profile 
            {...this.props} 
            node={node} 
            onNavigationChanged={component => this.setState({component})} 
            isLoading={this.state.isLoading}
          />          
        </WFace.WGrid>
        <WFace.WGrid item xl={10} lg={9} md={7} sm={6} xs={12}>          
          {this.state.component ? <this.state.component node={node} {...this.props}/> : <LoadingPage/>}
        </WFace.WGrid>
      </WFace.WGrid>
    )
  }
}