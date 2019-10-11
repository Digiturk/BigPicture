import Node from '../../base/node';
import { Modifier } from '../common';

export class Method extends Node {
  Assembly: string;
  NameSpace: string;
  OwnerName: string;
  Modifier: Modifier;
  HasBody: boolean;
}