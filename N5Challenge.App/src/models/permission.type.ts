import { PermissionType } from "./permissionType.type";

export type PermissionCreateRequest = {
  employeeFirstName: string;
  employeeLastName: string;
  permissionTypeId: number;
};

export type PermissionUpdateRequest = {
  employeeFirstName: string;
  employeeLastName: string;
  permissionTypeId: number;
  date: string;
};

export type PermissionUpdatePartialRequest = Partial<PermissionUpdateRequest>;

export type Permission = {
  id: number;
  employeeFirstName: string;
  employeeLastName: string;
  type: PermissionType;
  date: string;
};