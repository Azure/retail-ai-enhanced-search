import React, { useState } from 'react';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import DataCard from './DataCard';

export default function DataCardGrid({ data }) {
  const headers = Object.keys(data[0]);
  const rows = data.map(item => Object.values(item));

  return (
    <Box>
      <div class="grid place-items-stretch sm:grid-cols-2 sm:gap-2 md:grid-cols-3 md:gap-3 lg:grid-cols-5 lg:gap-4">
        {data.length &&
          data.map((row, index) => (
            <div class="" key={index}>
              <DataCard key={index} item={row} index={index}></DataCard>
            </div>
          ))
        }
      </div>
    </Box>
  );
}
