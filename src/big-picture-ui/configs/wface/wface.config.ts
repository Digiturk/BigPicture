import { IConfiguration, IHttpService } from "@wface/ioc";
import AuthService from '../../src/services/auth-service';
import AppHooks from '../../src/services/app-hooks';
import { ObjectBrowser } from '../../src/screens/object-browser';
import { WTheme } from '@wface/components';

const theme = {
} as WTheme

const config = {
  projectName: 'WFace',
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