import * as React from 'react';
import * as WFace from '@wface/components';
import Node from '../../base/node';

export default class GeneralInformation extends React.Component<{ node?: Node }, {}> {
  public render() {
    return (
      <WFace.WCard>
        <WFace.WCardHeader title={(
          <div style={{ display: 'flex', alignItems: 'center' }}>
            <WFace.WIcon>layers</WFace.WIcon>
            <WFace.WTypography variant="h5" style={{ flex: 1, marginLeft: 10 }}>{this.props.node.Name}</WFace.WTypography>
          </div>
        )} />
        <WFace.WCardContent>
          <div>
            <pre>
              {JSON.stringify(this.props.node, null, 2)}
            </pre>
          </div>
        </WFace.WCardContent>
      </WFace.WCard>
    );
  }
}