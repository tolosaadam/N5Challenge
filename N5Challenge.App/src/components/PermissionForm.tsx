import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  MenuItem,
  Backdrop,
} from '@mui/material';
import React, { useEffect } from 'react';
import { useForm, Controller } from 'react-hook-form';
import { Permission, PermissionCreateRequest, PermissionUpdateRequest } from '../models/permission.type';
import { PermissionType } from '../models/permissionType.type';
import { useCreatePermission } from '../hooks/useCreatePermission';
import { useUpdatePermission } from '../hooks/useUpdatePermission';

type Props = {
  open: boolean;
  onClose: () => void;
  permission: Permission | null;
  permissionTypes: PermissionType[];
};

type FormValues = {
  employeeFirstName: string;
  employeeLastName: string;
  typeId: number;
  date?: string;
};

const PermissionForm = ({ open, onClose, permission, permissionTypes }: Props) => {
  const isEditing = Boolean(permission);

  const { mutate: createPermission } = useCreatePermission();
  const { mutate: updatePermission } = useUpdatePermission();

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<FormValues>({
    defaultValues: {
      employeeFirstName: '',
      employeeLastName: '',
      typeId: permissionTypes[0]?.id ?? 0,
      date: '',
    },
  });

  useEffect(() => {
    if (permission) {
      reset({
        employeeFirstName: permission.employeeFirstName,
        employeeLastName: permission.employeeLastName,
        typeId: permission.type.id,
        date: permission.date.split('T')[0],
      });
    } else {
      reset({
        employeeFirstName: '',
        employeeLastName: '',
        typeId: permissionTypes[0]?.id ?? 0,
        date: '',
      });
    }
  }, [permission, permissionTypes, reset]);

  const handleClose = () => {
    onClose();
    reset();
  }

  const onSubmit = async (data: FormValues) => {
    if (isEditing) {
      const payload: PermissionUpdateRequest = {
        employeeFirstName: data.employeeFirstName,
        employeeLastName: data.employeeLastName,
        permissionTypeId: data.typeId,
        date: data.date ?? '',
      };

      updatePermission({ id: permission!.id, newPermission: payload });

    } else {
      const payload: PermissionCreateRequest = {
        employeeFirstName: data.employeeFirstName,
        employeeLastName: data.employeeLastName,
        permissionTypeId: data.typeId,
      };

      createPermission(payload);
    }
    onClose();
  };

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      BackdropComponent={Backdrop}
      BackdropProps={{
        style: {
          backdropFilter: 'blur(4px)',
        },
      }}
    >
      <DialogTitle>{isEditing ? 'Editar Permiso' : 'Crear Permiso'}</DialogTitle>
      <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 1, paddingTop: 24 }}>
        <Controller
          name="employeeFirstName"
          control={control}
          rules={{ required: 'El nombre es obligatorio' }}
          render={({ field }) => (
            <TextField
              {...field}
              label="Nombre"
              fullWidth
              error={!!errors.employeeFirstName}
              helperText={errors.employeeFirstName?.message}
              required
              sx={{ minHeight: 80, mt: 3 }}
            />
          )}
        />

        <Controller
          name="employeeLastName"
          control={control}
          rules={{ required: 'El apellido es obligatorio' }}
          render={({ field }) => (
            <TextField
              {...field}
              label="Apellido"
              fullWidth
              error={!!errors.employeeLastName}
              helperText={errors.employeeLastName?.message}
              required
              sx={{ minHeight: 80 }}
            />
          )}
        />

        <Controller
          name="typeId"
          control={control}
          rules={{ required: 'El tipo de permiso es obligatorio' }}
          render={({ field }) => (
            <TextField
              {...field}
              select
              label="Tipo de Permiso"
              fullWidth
              error={!!errors.typeId}
              helperText={errors.typeId?.message}
              required
              sx={{ minHeight: 80 }}
            >
              {permissionTypes.map(pt => (
                <MenuItem key={pt.id} value={pt.id}>
                  {pt.description}
                </MenuItem>
              ))}
            </TextField>
          )}
        />

        {isEditing && (
          <Controller
            name="date"
            control={control}
            rules={{ required: 'La fecha es obligatoria' }}
            render={({ field }) => (
              <TextField
                {...field}
                label="Fecha"
                type="date"
                fullWidth
                InputLabelProps={{ shrink: true }}
                error={!!errors.date}
                helperText={errors.date?.message}
                required
                sx={{ minHeight: 80 }}
              />
            )}
          />
        )}
      </DialogContent>

      <DialogActions sx={{ justifyContent: 'center' }}>
        <Button onClick={handleSubmit(onSubmit)} variant="contained" disabled={isSubmitting}>
          {isEditing ? 'Guardar Cambios' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default PermissionForm;
