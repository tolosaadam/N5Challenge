import axios from 'axios';
import { PermissionType } from '../models/permissionType.type';

export const getAllPermissionType = async (): Promise<PermissionType[]> => {
  const { data } = await axios.get('http://localhost:5230/permission-types');
  return data;
};
