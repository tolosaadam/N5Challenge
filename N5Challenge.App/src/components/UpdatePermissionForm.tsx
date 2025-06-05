import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import {
  TextField,
  Button,
  Select,
  MenuItem,
  InputLabel,
  FormControl
} from '@mui/material';

import { useUpdatePermission } from '../hooks/useUpdatePermission';
import { useGetAllPermissionType } from '../hooks/useGetAllPermissionTypes';
import { PermissionUpdateRequest } from '../models/permission.type';

interface UpdatePermissionFormProps {
  id: number;
  initialData: PermissionUpdateRequest;
  onSuccess: () => void;
}

const UpdatePermissionForm: React.FC<UpdatePermissionFormProps> = ({
  id,
  initialData,
  onSuccess
}) => {
  const { control, handleSubmit, register, formState: { errors } } =
    useForm<PermissionUpdateRequest>({
      defaultValues: initialData
    });

  const { data: permissionTypes, isLoading } = useGetAllPermissionType();
  const mutation = useUpdatePermission();

  const onSubmit = (data: PermissionUpdateRequest) => {
    mutation.mutate({ id, newPermission: data }, { onSuccess });
  };

  if (isLoading) return <div>Loading permission types...</div>;

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <TextField
        label="First Name"
        {...register('employeeFirstName', { required: 'First name is required' })}
        error={!!errors.employeeFirstName}
        helperText={errors.employeeFirstName?.message}
        fullWidth
        margin="normal"
      />

      <TextField
        label="Last Name"
        {...register('employeeLastName', { required: 'Last name is required' })}
        error={!!errors.employeeLastName}
        helperText={errors.employeeLastName?.message}
        fullWidth
        margin="normal"
      />

      <FormControl fullWidth margin="normal">
        <InputLabel>Permission Type</InputLabel>
        <Controller
          name="permissionTypeId"
          control={control}
          rules={{ required: 'Permission type is required' }}
          render={({ field }) => (
            <Select {...field} label="Permission Type" error={!!errors.permissionTypeId}>
              {permissionTypes?.map((type) => (
                <MenuItem key={type.id} value={type.id}>
                  {type.description}
                </MenuItem>
              ))}
            </Select>
          )}
        />
        {errors.permissionTypeId && (
          <p style={{ color: 'red' }}>{errors.permissionTypeId.message}</p>
        )}
      </FormControl>

      <TextField
        label="Permission Date"
        type="date"
        {...register('date', { required: 'Permission date is required' })}
        error={!!errors.date}
        helperText={errors.date?.message}
        fullWidth
        margin="normal"
        InputLabelProps={{ shrink: true }}
      />

      {mutation.isPending && <p>Updating permission...</p>}
      {mutation.isSuccess && <p style={{ color: 'green' }}>Permission updated successfully!</p>}
      {mutation.isError && <p style={{ color: 'red' }}>Error updating permission</p>}

      <Button type="submit" variant="contained" color="primary" disabled={mutation.isPending}>
        {mutation.isPending ? 'Updating...' : 'Update'}
      </Button>
    </form>
  );
};

export default UpdatePermissionForm;
