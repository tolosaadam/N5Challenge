import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updatePermission } from '../services/permission.service';
import { PermissionUpdateRequest } from '../models/permission.type';

export const useUpdatePermission = () => {
  const queryClient = useQueryClient();

  return useMutation<
    void,                   // Tipo resultado mutación (void si no importa el resultado)
    Error,                  // Tipo del error
    { id: number; newPermission: PermissionUpdateRequest } // Tipo de variables que recibe la mutación (debe incluir id)
  >({
    mutationFn: ({ id, newPermission }) => updatePermission(newPermission, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['permissions'] });
      alert('Permission updated successfully');
    },
    onError: () => {
      alert('Error updating permission');
    },
  });
};
