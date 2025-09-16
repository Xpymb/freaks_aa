import React from "react";
import { TablePagination, Box } from "@mui/material";
import styles from "./_styles.module.scss";

interface RaidPaginationProps {
  totalCount: number;
  page: number;
  pageSize: number;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
  isLoading?: boolean;
}

const RaidPagination: React.FC<RaidPaginationProps> = ({
  totalCount,
  page,
  pageSize,
  onPageChange,
  onPageSizeChange,
  isLoading = false,
}) => {
  const handleChangePage = (
    event: React.MouseEvent<HTMLButtonElement> | null,
    newPage: number
  ) => {
    if (!isLoading) {
      onPageChange(newPage);
    }
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    if (!isLoading) {
      const newPageSize = parseInt(event.target.value, 10);
      onPageSizeChange(newPageSize);
      onPageChange(0);
    }
  };

  return (
    <Box className={styles.paginationWrapper}>
      <TablePagination
        component="div"
        count={totalCount}
        page={page}
        onPageChange={handleChangePage}
        rowsPerPage={pageSize}
        onRowsPerPageChange={handleChangeRowsPerPage}
        rowsPerPageOptions={[5, 10, 25, 50]}
        labelRowsPerPage="Показывать на странице:"
        labelDisplayedRows={({ from, to, count }) =>
          `${from}-${to} из ${count !== -1 ? count : `больше чем ${to}`}`
        }
        classes={{
          root: styles.pagination,
          toolbar: styles.toolbar,
          spacer: styles.spacer,
          selectLabel: styles.selectLabel,
          select: styles.select,
          input: styles.input,
          selectIcon: styles.selectIcon,
          menuItem: styles.menuItem,
          displayedRows: styles.displayedRows,
          actions: styles.actions,
        }}
        sx={{
          "& .MuiTablePagination-toolbar": {
            paddingLeft: 0,
            paddingRight: 0,
            minHeight: "auto",
          },
          "& .MuiTablePagination-selectLabel": {
            margin: 0,
            color: "rgba(255, 255, 255, 0.7)",
            fontSize: "0.875rem",
          },
          "& .MuiTablePagination-select": {
            color: "white",
            fontSize: "0.875rem",
            "& .MuiOutlinedInput-notchedOutline": {
              borderColor: isLoading
                ? "rgba(255, 255, 255, 0.1)"
                : "rgba(255, 255, 255, 0.23)",
            },
            "&:hover .MuiOutlinedInput-notchedOutline": {
              borderColor: isLoading
                ? "rgba(255, 255, 255, 0.1)"
                : "rgba(255, 255, 255, 0.5)",
            },
            "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
              borderColor: isLoading ? "rgba(255, 255, 255, 0.1)" : "#00ff88",
            },
          },
          "& .MuiTablePagination-displayedRows": {
            color: "rgba(255, 255, 255, 0.7)",
            fontSize: "0.875rem",
            margin: 0,
          },
          "& .MuiTablePagination-actions": {
            "& .MuiIconButton-root": {
              color: "rgba(255, 255, 255, 0.7)",
              "&:hover": {
                color: isLoading ? "rgba(255, 255, 255, 0.3)" : "#00ff88",
                backgroundColor: isLoading
                  ? "transparent"
                  : "rgba(0, 255, 136, 0.1)",
              },
              "&.Mui-disabled": {
                color: "rgba(255, 255, 255, 0.3)",
              },
            },
          },
        }}
      />
    </Box>
  );
};

export default RaidPagination;
