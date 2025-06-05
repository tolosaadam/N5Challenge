import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createPermission } from '../services/permission.service';
import { PermissionCreateRequest } from '../models/permission.type';

export const useCreatePermission = () => {
  const queryClient = useQueryClient();

  return useMutation<
    void,                    // Tipo del resultado que devuelve la mutación (aquí void si no importa el resultado)
    Error,                   // Tipo del error
    PermissionCreateRequest  // Tipo de variables que recibe la mutación
  >({
    mutationFn: createPermission,
    onSuccess: () => {
      queryClient.invalidateQueries({queryKey: ['permissions']});
      alert('Permission created successfully');
    },
    onError: () => {
      alert('Error creating permission');
    },
  });
};
