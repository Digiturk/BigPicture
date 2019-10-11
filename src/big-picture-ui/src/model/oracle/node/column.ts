import Node from '../../base/node';

export class Column extends Node {
  DatabaseName: string;
  Schema: string;
  TableName: string;

  ColumnId: number;
  DataType: string;
  DataLength: number;
  DataPrecision: number;
  DataScale: number;
  Nullable: string;
}