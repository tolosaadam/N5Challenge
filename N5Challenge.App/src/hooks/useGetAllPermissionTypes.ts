import { useQuery } from '@tanstack/react-query';
import { PermissionType } from '../models/permissionType.type';
import { getAllPermissionType } from '../services/permissionType.service';

export const useGetAllPermissionType = () => {
  return useQuery<PermissionType[], Error>({
    queryKey: ['permission-types'],
    queryFn: getAllPermissionType,
    staleTime: 1 * 60 * 1000,
    refetchOnWindowFocus: false
  });
};
