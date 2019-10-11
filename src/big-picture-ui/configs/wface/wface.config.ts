import { IConfiguration } from "@wface/ioc";
import AuthService from '../../src/services/auth-service';
import AppHooks from '../../src/services/app-hooks';
import { NodeBrowser } from '../../src/screens/node-browser';
import { WTheme } from '@wface/components';
import NodeHttpService from '../../src/services/node-http-service';
import NodeSearhProvider from '../../src/services/node-search-provider';

const theme = {
  designDetails: {
    pagePadding: 10
  },
  palette: {
    primary: { main: '#1e1e2d' },
    secondary: { main: '#5d78ff' },
    text: { primary: '#65819d' }
  },
  props: {
    MuiCard: {
      elevation: 1
    }
  },
  overrides: {    
    MuiCard: {
      root: {
        boxShadow: '0px 0px 13px 0px rgba(82, 63, 105, 0.1)',                
      },
    }
  }
} as WTheme

const config = {
  projectName: 'BigPicture',
  screenList: {
    NodeBrowser,    
  },    
  authService: AuthService,
  theme: theme,
  useLocalStorage: true,
  hooks: AppHooks,
  search: NodeSearhProvider,  
  httpService: NodeHttpService,
} as IConfiguration

export default config;