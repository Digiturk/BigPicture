import * as React from 'react';
import * as WFace from '@wface/components';
import Entity from './entity';

export default class Node extends Entity {
  name: string;
  labels: string[];

  public getShortLabel = (): string => this.labels[0].substr(0, 2)

  public getIcon = (size: number = 40): React.ReactElement => {
    const shortLabel = this.getShortLabel();    
    const backgroundColor = this.stringToColor(this.labels[0]);
    const fontSize = `calc(1.25rem * ${size / 40}`; 
    // const color = this.
    return <WFace.WAvatar title={this.name} style={{backgroundColor, width: size, height: size, fontSize}}>{shortLabel}</WFace.WAvatar>
  }

  private stringToColor = (text: string):string => {
    let hash = 0;
    let i;
  
    /* eslint-disable no-bitwise */
    for (i = 0; i < text.length; i += 1) {
      hash = text.charCodeAt(i) + ((hash << 5) - hash);
    }
  
    let color = '#';
  
    for (i = 0; i < 3; i += 1) {
      const value = (hash >> (i * 8)) & 0xff;
      color += `00${value.toString(16)}`.substr(-2);
    }
    /* eslint-enable no-bitwise */
  
    return color;
  }
}