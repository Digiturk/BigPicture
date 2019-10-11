import * as React from 'react';
import * as WFace from '@wface/components';
import { Profile } from '../../components/node-components/profile';
import LoadingPage from './loading-page';
import Node from '../../model/base/node';


interface NodeBrowserState {
  component: React.ReactType<{node?: Node}>;
}

export class NodeBrowser extends React.Component<WFace.BaseScreenProps, NodeBrowserState> {
  constructor(props: WFace.BaseScreenProps) {
    super(props);

    this.state = this.props.screenData.state || {
      component: null
    }
  }

  public render() {
    return (
      <WFace.WGrid container spacing={2}>
        <WFace.WGrid item xl={2} lg={3} md={5} sm={6} xs={12}>          
          <Profile {...this.props} node={this.props.screenData.initialValues.node} onNavigationChanged={component => this.setState({component})}/>          
        </WFace.WGrid>
        <WFace.WGrid item xl={10} lg={9} md={7} sm={6} xs={12}>          
          {this.state.component ? <this.state.component node={this.props.screenData.initialValues.node}/> : <LoadingPage/>}
        </WFace.WGrid>
      </WFace.WGrid>
    )
  }
}