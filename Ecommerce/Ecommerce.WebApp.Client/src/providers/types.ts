export interface Roles {
  role: string;
  permissions: Permission[];
}

export interface Permission {
  resource: string;
  action: string[];
}

export interface ResponseRoot {
  Succeeded: boolean;
  Code: number;
  Message: string;
  Errors: any;
  Data: ResponseData;
}

export interface ResponseData {
  _start: number;
  _pages: number;
  _end: number;
  _total: number;
  _hasNext: boolean;
  _hasPrevious: boolean;
  _data: any[];
}

export interface ResponseManyRoot {
  Succeeded: boolean;
  Code: number;
  Message: string;
  Errors: any;
  Data: any[];
}
