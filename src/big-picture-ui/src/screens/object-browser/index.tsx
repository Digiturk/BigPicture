import * as React from 'react';
import * as WFace from '@wface/components';

interface ObjectBrowserState {
}

export class ObjectBrowser extends React.Component<WFace.BaseScreenProps, ObjectBrowserState> {
  constructor(props: WFace.BaseScreenProps) {
    super(props);

    this.state = this.props.screenData.state || {
    }
  }

  public render() {
    return (
      <>
        Object browser
      </>
    )
  }
}