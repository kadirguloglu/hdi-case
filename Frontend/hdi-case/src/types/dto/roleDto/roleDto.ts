export default interface RoleDto {
  id: string;
  name: string;
  description: string;
  admins: Array<number>;
  permissionKeys: Array<number>;
}
