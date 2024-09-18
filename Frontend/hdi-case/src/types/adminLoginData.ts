import { TableEntities } from "./base/tableEntities";
export interface AdminLoginData extends TableEntities<AdminLoginData> {
  email: string;
  password: string;
  isDeveloper: boolean;
  roleId: string[] | null;
}
