import Node from '../../base/node';
import GeneralInformation from '../general-information';
import { CodeEditor } from '../code-editor';

export default interface NavigationItem {
  icon: string;
  text: string;
  component: React.ReactType<{ node?: Node }>;
}

export const NavigationItems: { [key: string]: NavigationItem } = {
  generalInformation: {
    icon: 'layers',
    text: 'General Information',
    component: GeneralInformation
  },
  code: {
    icon: 'code',
    text: 'Code',
    component: CodeEditor
  }
}