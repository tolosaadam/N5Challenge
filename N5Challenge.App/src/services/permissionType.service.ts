import axios from 'axios';
import { PermissionType } from '../models/permissionType.type';

const API_URL = process.env.REACT_APP_API_URL;

export const getAllPermissionType = async (): Promise<PermissionType[]> => {
  const { data } = await axios.get(`${API_URL}permission-types`);
  return data;
};
