import { Result } from "../../types/abstract/result";
import PermissionCategoryList from "../../types/dto/permissionDto/permissionCategoryList";
import RoleDto from "../../types/dto/roleDto/roleDto";
import { axiosInstance } from "../client";

export const getPermissions = async () => {
  return await axiosInstance.get<Array<PermissionCategoryList>>(
    "/api/api/v1/permission/GetPermissions"
  );
};

export const updateRole = async (model: RoleDto) => {
  return await axiosInstance.post<Result<boolean>>(
    "/api/api/v1/Role/UpdateRole",
    model
  );
};

export const getRoleById = async (id: number) => {
  return await axiosInstance.get<Result<RoleDto>>(
    `/api/api/v1/Role/GetRoleById/${id}`
  );
};
