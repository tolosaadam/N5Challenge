import React, { useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { Button, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material';
import { PermissionCreateRequest } from '../models/permission.type';
import { useCreatePermission } from '../hooks/useCreatePermission';
import { useGetAllPermissionType } from '../hooks/useGetAllPermissionTypes';

interface CreatePermissionFormProps {
  onSuccess: () => void;
}

const CreatePermissionForm: React.FC<CreatePermissionFormProps> = ({ onSuccess }) => {
  const { data: permissionTypes, isLoading } = useGetAllPermissionType();
  const { control, register, handleSubmit, formState: { errors }, reset } = useForm<PermissionCreateRequest>();
  const createMutation = useCreatePermission();

  useEffect(() => {
    if (createMutation.isSuccess) {
      reset();
    }
  }, [createMutation.isSuccess, reset]);

  const onSubmit = (data: PermissionCreateRequest) => {
    createMutation.mutate(data, {
      onSuccess: () => {
        onSuccess();
      }
    });
  };

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
              {isLoading ? (
                <MenuItem disabled>Cargando...</MenuItem>
              ) : (
                (permissionTypes || []).map((type) => (
                  <MenuItem key={type.id} value={type.id}>
                    {type.description}
                  </MenuItem>
                ))
              )}
            </Select>
          )}
        />
        {errors.permissionTypeId && <p style={{ color: 'red' }}>{errors.permissionTypeId.message}</p>}
      </FormControl>

      <div style={{ minHeight: 24, marginBottom: 8 }}>
        {createMutation.isPending && <p>Creating permission...</p>}
        {createMutation.isSuccess && <p style={{ color: 'green' }}>Permission created successfully!</p>}
        {createMutation.isError && <p style={{ color: 'red' }}>Error creating permission</p>}
      </div>


      <Button type="submit" variant="contained" color="primary" disabled={createMutation.isPending}>
        {createMutation.isPending ? 'Creating...' : 'Create'}
      </Button>
    </form>
  );
};

export default CreatePermissionForm;


