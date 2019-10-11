import Node from '../../base/node';

export class Project extends Node {
  AbsolutePath: string;
  ProjectGuid: string;
  RelativePath: string;
  OutputType: string;
  AssemblyName: string;
  TargetFrameworkVersion: string;
}