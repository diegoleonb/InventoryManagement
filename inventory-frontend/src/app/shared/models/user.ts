// Data Transfer Objects and interfaces for user authentication and roles
export interface UserLoginDto {
  username: string;
  password: string;
}

export interface UserRegisterDto {
  username: string;
  email: string;
  password: string;
  roleId: string;
}

export interface UserResponseDto {
  id: number;
  username: string;
  email: string;
  roleId: string;
  roleName: string;
  token: string;
}

export interface RoleDto {
  id: string;
  name: string;
}
