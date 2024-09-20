export default interface PermissionList {
  key: number;
  value: string;
  childPermission: Array<PermissionList> | null;
}
