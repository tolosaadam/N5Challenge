import axios from 'axios';
import { Permission, PermissionCreateRequest } from '../models/permission.type';

const API_URL = process.env.REACT_APP_API_URL;

export const createPermission = async (newPermission: PermissionCreateRequest) => {
  const { data } = await axios.post(`${API_URL}permissions`, newPermission);
  return data;
};

export const updatePermission = async (newPermission: PermissionCreateRequest, id: number) => {
  const { data } = await axios.put(`${API_URL}permissions/${id}`, newPermission);
  return data;
};

export const getAllPermission = async (): Promise<Permission[]> => {
  const { data } = await axios.get(`${API_URL}permissions`);
  return data;
};

export const getPermissionById = async (id: number): Promise<Permission> => {
  const { data } = await axios.get(`${API_URL}permissions/${id}`);
  return data;
};
