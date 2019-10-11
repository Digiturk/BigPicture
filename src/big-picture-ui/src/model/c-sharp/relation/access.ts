import Relation from '../../base/relation';

export class Access extends Relation {
  Code: string;
  ParamNames: string[];
  ParamValues: string[];
  ParamCodes: string[];
}