import PermissionList from "./permissionList";

export default interface PermissionCategoryList {
  categoryKey: number;
  categoryName: string;
  permissions: Array<PermissionList> | null;
}
