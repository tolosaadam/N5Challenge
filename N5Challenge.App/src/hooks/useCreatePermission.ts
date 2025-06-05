import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createPermission } from '../services/permission.service';
import { PermissionCreateRequest } from '../models/permission.type';

export const useCreatePermission = () => {
  const queryClient = useQueryClient();

  return useMutation<
    void,
    Error,
    PermissionCreateRequest
  >({
    mutationFn: createPermission,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['permissions'] });
      alert('Permiso creado satisfactoriamente');
    },
    onError: () => {
      alert('Error creando permiso');
    },
  });
};
