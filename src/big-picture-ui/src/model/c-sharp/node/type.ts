import Node from '../../base/node';
import { Modifier } from '../common';

export class Type extends Node {
  Assembly: String;
  NameSpace: String;
  Modifier: Modifier;
}