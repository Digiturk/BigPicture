import * as React from 'react';
import * as WFace from '@wface/components';
import Entity from './entity';
import NavigationItem, { NavigationItems } from '../components/base/navigation-item';
import GeneralInformation from '../components/general-information';

export default class Node extends Entity {
  Name: string;
  Labels: string[];

  public getShortLabel (): string { 
    return this.Labels[0].substr(0, 2);
  }

  public getIcon(size: number = 40): React.ReactElement {
    const shortLabel = this.getShortLabel();
    const backgroundColor = this.stringToColor(this.Labels[0]);
    const fontSize = `calc(1.25rem * ${size / 40})`;
    return <WFace.WAvatar title={this.Name} style={{ backgroundColor, width: size, height: size, fontSize }}>{shortLabel}</WFace.WAvatar>
  }

  public getNavigationItems(): NavigationItem[] {
    return [ NavigationItems.generalInformation ];
  }

  private stringToColor = (text: string): string => {
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