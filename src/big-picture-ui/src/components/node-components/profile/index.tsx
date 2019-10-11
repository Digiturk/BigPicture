import * as React from 'react';
import * as WFace from '@wface/components';
import { fade } from '@material-ui/core/styles';
import { createStyles, withStyles } from '@material-ui/styles';
import { WTheme } from '@wface/components';
import classNames from 'classnames';
import Node from '../../../model/base/node';
import { getNodeImplementation } from '../../../model/base';
import NavigationItem from '../../../model/components/base/navigation-item';


interface ProfileState {  
  navSelectedIndex: number;
  navItems: NavigationItem[];    
}

export interface ProfileProps extends WFace.BaseScreenProps {
  classes?: any;
  node: Node;
  onNavigationChanged: (component: React.ReactType<{ node?: Node }>) => void;
  isLoading: boolean;
}

class ProfileInner extends React.Component<ProfileProps, ProfileState> {
  constructor(props: ProfileProps) {
    super(props);

    this.state = {
      navItems: this.props.node.getNavigationItems(),
      navSelectedIndex: 0,      
    }
  }

  componentDidUpdate(prevProps: ProfileProps, prevState, snapshot) {
    if (prevProps.node !== this.props.node) {
      this.setState({
        navItems: this.props.node.getNavigationItems(),
        navSelectedIndex: 0
      }, () => this.props.onNavigationChanged(this.state.navItems[0].component))
    }
  }

  private renderIcon = () => {
    const { classes } = this.props;

    return (
      <div className={classes.iconWrapper}>
        {this.props.node.getIcon(100)}
      </div>
    );
  }

  private renderLabels = () => {
    const { classes } = this.props;


    return (
      <div className={classes.labelsContainer}>
        {this.props.node.Labels.map(label => (
          <WFace.WTypography variant="caption" className={classes.label}>
            {label}
          </WFace.WTypography>
        ))}
      </div>
    )
  }

  private renderNav = () => {
    const { classes } = this.props;

    if (this.props.isLoading) {
      var skeleton = (
        <div style={{ display: 'flex' }}>
          <WFace.WSkeleton height={36} width={36} style={{ margin: 5 }} />
          <WFace.WSkeleton height={36} style={{ flex: 1, margin: 5 }} />
        </div>
      );
      return (
        <div className={classes.navList}>
          {skeleton}
          {skeleton}
          {skeleton}
        </div>
      );
    }
    else {
      return (
        <WFace.WList id="node-navigator" className={classes.navList}>
          {this.state.navItems.map((item, index) => (
            <WFace.WListItem
              className={classNames(classes.navListItem, { [classes.navListItemSelected]: index === this.state.navSelectedIndex })}
              selected={index === this.state.navSelectedIndex}
              button
              onClick={() => this.setState({ navSelectedIndex: index }, () => this.props.onNavigationChanged(item.component))}
            >
              <WFace.WListItemIcon className={classes.navListItemIcon}>
                <WFace.WIcon iconSize="small">{item.icon}</WFace.WIcon>
              </WFace.WListItemIcon>
              <WFace.WListItemText>
                {item.text}
              </WFace.WListItemText>
            </WFace.WListItem>
          ))}
        </WFace.WList>
      );
    }
  }

  public render() {
    const { classes } = this.props;

    return (
      <WFace.WCard>
        <WFace.WCardContent>
          {this.renderIcon()}
          <WFace.WTypography align="center" variant="h6" className={classes.title}>{this.props.node.Name}</WFace.WTypography>
          {this.renderLabels()}
          {this.renderNav()}
        </WFace.WCardContent>
      </WFace.WCard>
    );
  }
}

const backgroundColor = (theme: WTheme, opacity: number = 0.6) => fade(theme.palette.background.default, opacity);

const styles = (theme: WTheme) => createStyles({
  iconWrapper: {
    display: 'flex',
    justifyContent: 'center',
    marginTop: 20,
  },
  label: {
    background: theme.palette.primary.light,
    borderRadius: 4,
    fontSize: 10,
    padding: '2px 6px',
    margin: '0 2px'
  },
  labelsContainer: {
    display: 'flex',
    justifyContent: 'center',
    color: 'white'
  },
  navList: {
    marginTop: 20
  },
  navListItem: {
    borderRadius: 4,
    margin: '2px 0',
    '&:hover': {
      backgroundColor: backgroundColor(theme),
    }
  },
  navListItemIcon: {
    color: theme.palette.text.primary,
    opacity: 0.75,
    marginRight: 6,
    minWidth: 24,
  },
  navListItemSelected: {
    color: theme.palette.text.primary,
    backgroundColor: backgroundColor(theme) + ' !important',
  },
  title: {
    marginTop: theme.spacing(2),
    wordBreak: 'break-all'
  }
});

export const Profile = withStyles(styles)(ProfileInner); 