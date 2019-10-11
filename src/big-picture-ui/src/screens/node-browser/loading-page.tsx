import * as React from 'react';
import * as WFace from '@wface/components';

export default class LoadingPage extends React.Component<{}, {}> {
  render() {
    return (
      <WFace.WCard>
        <WFace.WCardHeader title={
          <>
            <WFace.WSkeleton height={40} width={40} variant="circle" style={{ display: 'inline-block' }} />
            <WFace.WSkeleton height={20} width={200} variant="rect" style={{ display: 'inline-block', marginLeft: 20, marginBottom: 10 }} />
          </>
        } />
        <WFace.WCardContent>
          <WFace.WGrid container spacing={3}>
            <WFace.WGrid item xs={3}>
              <WFace.WSkeleton height={185} />
            </WFace.WGrid>
            <WFace.WGrid item xs={9}>
              <WFace.WSkeleton height={20} />
              <WFace.WSkeleton height={20} width="80%" />
              <WFace.WSkeleton height={20} width="70%" />
              <WFace.WSkeleton height={20} width="90%" />
              <WFace.WSkeleton height={20} width="60%" />
              <WFace.WSkeleton height={20} width="90%" />
            </WFace.WGrid>
          </WFace.WGrid>
          
          <WFace.WSkeleton height={200}/>

        </WFace.WCardContent>
        <WFace.WCardActions>
            <WFace.WSkeleton height={36} width={100} style={{margin: '0 5px'}}/>
            <WFace.WSkeleton height={36} width={100} style={{margin: '0 5px'}}/>
          </WFace.WCardActions>
      </WFace.WCard>
    );
  }
}