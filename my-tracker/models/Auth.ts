export interface RegistrationCommand {
  nickName: string;
  email: string;
  password: string;
}

export interface LoginCommand{
    email: string;
    password: string;
}