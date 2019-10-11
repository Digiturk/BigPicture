import Node from '../../base/node';

export class DbObject extends Node {
  DatabaseName: string;
  SchemaName: string;
  Type: string;
}