import { useQuery } from '@tanstack/react-query';
import { Permission } from '../models/permission.type';
import { getAllPermission } from '../services/permission.service';

export const useGetAllPermission = () => {
  return useQuery<Permission[], Error>({
    queryKey: ['permissions'],
    queryFn: getAllPermission,
    staleTime: 1 * 60 * 1000,
    refetchOnWindowFocus: false
  });
};
