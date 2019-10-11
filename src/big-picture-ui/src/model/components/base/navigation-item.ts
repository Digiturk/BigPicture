import Node from '../../base/node';

export default interface NavigationItem {
  icon: string;
  text: string;
  component: React.ReactType<{node?: Node}>;
}