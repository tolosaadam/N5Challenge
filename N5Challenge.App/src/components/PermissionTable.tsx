import { Table, TableHead, TableRow, TableCell, TableBody, IconButton, Box, TableContainer, Paper } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import React from 'react';
import { Permission } from '../models/permission.type';

type Props = {
  permissions: Permission[];
  onEdit: (permission: Permission) => void;
};

const PermissionTable = ({ permissions, onEdit }: Props) => {


  return (
    <TableContainer component={Paper} sx={{ maxHeight: 400, overflow: 'auto' }}>
      <Table stickyHeader size="small">
        <TableHead>
          <TableRow sx={{ backgroundColor: '#424242' }}>
            {['Nombre', 'Apellido', 'Tipo Permiso', 'Fecha', 'Acciones'].map((header, i) => (
              <TableCell
                key={i}
                sx={{
                  backgroundColor: '#424242',
                  color: 'white',
                  fontWeight: 'bold',
                }}
              >
                {header}
              </TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {permissions.map(permission => (
            <TableRow key={permission.id} hover sx={{ backgroundColor: 'white' }}>
              <TableCell>{permission.employeeFirstName}</TableCell>
              <TableCell>{permission.employeeLastName}</TableCell>
              <TableCell>{permission.type.description}</TableCell>
              <TableCell>{new Date(permission.date).toLocaleDateString()}</TableCell>
              <TableCell>
                <IconButton onClick={() => onEdit(permission)}>
                  <EditIcon />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default PermissionTable;

