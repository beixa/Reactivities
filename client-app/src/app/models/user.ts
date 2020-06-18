export interface IUser {
  username: string;
  displayName: string;
  token: string;
  refreshToken: string;
  image?: string;
}

export interface IUserFormValues {
  email: string;
  password: string;
  displayname?: string;
  username?: string;
}
