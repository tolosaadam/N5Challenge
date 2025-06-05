import { useQuery } from '@tanstack/react-query';
import { Permission } from '../models/permission.type';
import { getPermissionById } from '../services/permission.service';

export const useGetPermissionById = (id: number) => {
    return useQuery<Permission, Error>({
        queryKey: ['permissions', id],
        queryFn: () => getPermissionById(id),
        //enabled: !!id, // evita que se ejecute si id es null o undefined
        //staleTime: 1 * 60 * 1000,
        //refetchOnWindowFocus: false
    });
};
