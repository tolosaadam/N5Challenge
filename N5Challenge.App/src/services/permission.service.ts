import axios from 'axios';
import { Permission, PermissionCreateRequest } from '../models/permission.type';

export const createPermission = async (newPermission: PermissionCreateRequest) => {
  const { data } = await axios.post('http://localhost:5230/permissions', newPermission);
  return data;
};

export const updatePermission = async (newPermission: PermissionCreateRequest, id: number) => {
  const { data } = await axios.put(`http://localhost:5230/permissions/${id}`, newPermission);
  return data;
};

export const getAllPermission = async (): Promise<Permission[]> => {
  const { data } = await axios.get('http://localhost:5230/permissions');
  return data;
};
