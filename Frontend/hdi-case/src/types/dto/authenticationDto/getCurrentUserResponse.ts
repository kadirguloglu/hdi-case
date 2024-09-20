import { AdminLoginData } from "../../adminLoginData";
import { Role } from "../../role";

export interface GetCurrentUserResponse {
  adminLoginData: AdminLoginData;
  roles: Role[];
}
