import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updatePermission } from '../services/permission.service';
import { Permission, PermissionUpdateRequest } from '../models/permission.type';
import { PermissionType } from '../models/permissionType.type';

export const useUpdatePermission = () => {
  const queryClient = useQueryClient();

  return useMutation<
    void,
    Error,
    { id: number; newPermission: PermissionUpdateRequest },
    Permission[]
  >({
    mutationFn: ({ id, newPermission }) => updatePermission(newPermission, id),
    onMutate: async ({ id, newPermission }) => {

      // Cancel any outgoing refetches
      // (so they don't overwrite our optimistic update)
      await queryClient.cancelQueries({ queryKey: ['permissions', id] });

      // Snapshot the previous value
      const previousPermissions = queryClient.getQueryData<Permission[]>(['permissions']);

      const permissionTypes = queryClient.getQueryData<PermissionType[]>(['permission-types']);

      // Buscar el PermissionType correspondiente al id
      const matchedPermissionType = permissionTypes?.find(
        pt => pt.id === newPermission.permissionTypeId
      );

      if (!matchedPermissionType) {
        throw new Error('No se encontró el tipo de permiso en cache');
      }

      queryClient.setQueryData<Permission[]>(['permissions'], old =>
        old?.map(p =>
          p.id === id ?
            {
              ...p,
              employeeFirstName: newPermission.employeeFirstName,
              employeeLastName: newPermission.employeeLastName,
              type: matchedPermissionType,
              date: newPermission.date

            } : p
        ));

      // Return a context with the previous and new todo
      return previousPermissions
    },
    // If the mutation fails, use the context we returned above
    onError: (err, { id, newPermission }, previousPermissions) => {
      queryClient.setQueryData(
        ['permissions'],
        previousPermissions,
      );

      alert('Error actualizando permiso');
    },
    onSuccess: () => {
      alert('Permission actualizado satisfactoriamente');
    }
  });
};
