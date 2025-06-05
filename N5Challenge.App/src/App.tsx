// src/App.tsx

import React from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { CssBaseline, Box } from '@mui/material';
import PermissionPage from './pages/PermissionPage';

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <CssBaseline />
      <Box sx={{ bgcolor: '#e0e0e0', height: '100vh' }}>
        <PermissionPage />
      </Box>
    </QueryClientProvider>
  );
}

export default App;
