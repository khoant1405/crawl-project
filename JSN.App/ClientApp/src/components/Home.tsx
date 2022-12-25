import { Box, Pagination, Stack, Typography } from '@mui/material';
import * as React from 'react';
import { connect } from 'react-redux';
import TablePagination from '@mui/material/TablePagination';

export default function Home() {
  const [page, setPage] = React.useState(1);
  const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };

  return (
    <Box sx={{ p: 3, mt: 8, mb: 1 }}>
      <Stack spacing={2}>
        <Pagination count={10} page={page} onChange={handleChange} color="secondary" />
      </Stack>
    </Box>
  );
}
