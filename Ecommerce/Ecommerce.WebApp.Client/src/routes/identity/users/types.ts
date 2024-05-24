export interface IUserShort {
  Id: string;
  RoleId: string;
  UserName: string;
  FirstName: string;
  LastName: string;
  Email: string;
  PhoneNumber: string;
  AvatarUrl: string;
}

export interface IUser extends IUserShort {
  RefreshTokens: any;
  Id: string;
  NormalizedUserName: string;
  NormalizedEmail: string;
  EmailConfirmed: boolean;
  PasswordHash: string;
  SecurityStamp: string;
  ConcurrencyStamp: string;
  PhoneNumberConfirmed: boolean;
  TwoFactorEnabled: boolean;
  LockoutEnd: any;
  LockoutEnabled: boolean;
  AccessFailedCount: number;
  Avatar: IUserAvatar;
}

export interface IUserByMe {
  Email: string;
  Name: string;
  Fullname: string;
  Roles: string[];
  Uid: string;
  AvatarUrl: string;
  AvatarUid: string;
}

export interface ICreateUser {
  FirstName: string;
  LastName: string;
  UserName: string;
  Email: string;
  PhoneNumber: any;
  Password: string;
  ConfirmPassword: string;
}

export interface IUserAvatar {
  AvatarName: string;
  AvatarUid: string;
  AvatarUrl: string;
}

export interface IUserLdapInfo {
  EmailAddress: string;
  Name: any;
  DisplayName: string;
  DistinguishedName: any;
  EmployeeId: any;
  GivenName: any;
  MiddleName: any;
  Surname: any;
  VoiceTelephoneNumber: any;
}
