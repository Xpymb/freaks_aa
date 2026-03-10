import {
  DataGrid,
  type GridApi,
  GridColDef,
  useGridApiRef,
} from "@mui/x-data-grid";
import type React from "react";
import styles from "./_styled.module.scss";

function FooterSlot({ children }: { children?: React.ReactNode }) {
  if (!children) return null;
  return <>{children}</>;
}

type Props<R extends object> = {
  rows: R[];
  columns: GridColDef<R>[];
  loading?: boolean;
  footer?: React.ReactNode;
  pageSize?: number;
  apiRef?: React.RefObject<GridApi | null>;
  fillHeight?: boolean;
  getRowId?: (row: R) => number | string;
};

export default function CustomDataGrid<R extends object>({
  rows,
  columns,
  loading,
  footer,
  pageSize = 25,
  apiRef: apiRefProp,
  fillHeight,
  getRowId,
}: Props<R>) {
  const internalRef = useGridApiRef();
  const apiRef = apiRefProp ?? internalRef;

  return (
    <div className={fillHeight ? styles.wrapperFill : styles.wrapper}>
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        // pageSizeOptions={[pageSize]}
        initialState={{
          pagination: { paginationModel: { pageSize } },
        }}
        {...(!fillHeight && { autoHeight: true })}
        {...(getRowId && { getRowId })}
        localeText={{ noRowsLabel: "Нет данных" }}
        disableRowSelectionOnClick
        hideFooter={!footer}
        slots={footer ? { footer: FooterSlot } : undefined}
        slotProps={footer ? { footer: { children: footer } } : undefined}
        apiRef={apiRef}
        sx={{ border: 0, ...(fillHeight && { height: "100%" }) }}
      />
    </div>
  );
}
