import axios, { AxiosInstance } from 'axios';
import { injectable, inject } from "inversify";
import { IHttpService } from '@wface/ioc';
import { UserContext } from '@wface/store';

@injectable()
export default class NodeHttpService implements IHttpService {
  
  axiosInstance: AxiosInstance;
  @inject("UserContext") userContext: UserContext;
  
  constructor() {
    this.axiosInstance = axios.create({
      baseURL: 'http://localhost:62616/api/',
    })
  }
  
  getConfig = () => {}    

  public get<T = any>(url: string, params?: {}): Promise<T> {
    return new Promise<T>((resolve, reject) => {
      this.axiosInstance.get<T>(url, { params })      
        .then(response => resolve(response.data))
        .catch(error => reject(error))
    });
  }

  public post<T = any>(url: string, data?: {}): Promise<T> {
    return new Promise<T>((resolve, reject) => {
      this.axiosInstance.post(url, data)      
        .then(response => resolve(response.data as T))
        .catch(error => reject(error))
    })
  }
}