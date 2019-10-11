import Node from '../../base/node';
import { Modifier } from '../common';

export class Field extends Node {
  Assembly: string;
  NameSpace: string;
  OwnerName: string;
  Modifier: Modifier;
}