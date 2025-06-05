import { CircularProgress, Typography, Container, Box, Button } from '@mui/material';
import { useState } from 'react';
import { Permission } from '../models/permission.type';
import { useGetAllPermissionType } from '../hooks/useGetAllPermissionTypes';
import { useGetAllPermission } from '../hooks/useGetAllPermissions';
import React from 'react';
import PermissionTable from '../components/PermissionTable';
import PermissionForm from '../components/PermissionForm';

const PermissionPage = () => {

  const {
    data: permissions,
    isLoading: isLoadingPermissions,
    isError: isErrorPermissions,
    error: errorPermissions,
  } = useGetAllPermission();

  const {
    data: permissionTypes,
    isLoading: isLoadingPermissionTypes,
    isError: isErrorPermissionTypes,
    error: errorPermissionTypes,
  } = useGetAllPermissionType();

  const isLoading = isLoadingPermissions || isLoadingPermissionTypes;
  const isError = isErrorPermissions || isErrorPermissionTypes;

  let errorMessage = 'Hubo un problema';
  if (isErrorPermissions) errorMessage = `Error cargando permisos: ${errorPermissions?.message}`;
  else if (isErrorPermissionTypes) errorMessage = `Error cargando tipos de permiso: ${errorPermissionTypes?.message}`;

  // Estados para el modal etc
  const [selectedPermission, setSelectedPermission] = useState<Permission | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleOpenModal = (permission?: Permission) => {
    setSelectedPermission(permission ?? null);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setSelectedPermission(null);
    setIsModalOpen(false);
  };

  return (
    <Container sx={{ textAlign: 'center', pt: 4 }}>
      {isLoading ? (
        <CircularProgress />
      ) : isError ? (
        <Typography color="error">{errorMessage}</Typography>
      ) : (
        <>
          <Typography variant="h4" gutterBottom>
            Permisos
          </Typography>

          <Box sx={{ display: 'flex', justifyContent: 'flex-end', pb: 2 }}>
            <Button variant="contained" onClick={() => handleOpenModal()}>
              Crear Permiso
            </Button>
          </Box>

          <PermissionTable
            permissions={permissions ?? []}
            onEdit={handleOpenModal}
          />

          <PermissionForm
            open={isModalOpen}
            onClose={handleCloseModal}
            permission={selectedPermission}
            permissionTypes={permissionTypes ?? []}
          />
        </>
      )}
    </Container>
  );
};

export default PermissionPage;
