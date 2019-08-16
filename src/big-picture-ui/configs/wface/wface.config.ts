import { IConfiguration, IHttpService } from "@wface/ioc";
import AuthService from '../../src/services/auth-service';
import AppHooks from '../../src/services/app-hooks';
import { ObjectBrowser } from '../../src/screens/object-browser';
import { WTheme } from '@wface/components';

const theme = {
  palette: {
    primary: { main: '#388e3c'},
  },
  designDetails: {
    pagePadding: 10
  }
} as WTheme

const config = {
  projectName: 'BigPicture',
  screenList: {
    ObjectBrowser,    
  },    
  authService: AuthService,
  theme: theme,
  useLocalStorage: true,
  hooks: AppHooks,
  search: true,  
} as IConfiguration

export default config;