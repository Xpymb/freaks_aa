import { useEffect, useState } from "react";
import type { GridApi } from "@mui/x-data-grid";
import type React from "react";

export function useColumnWidths(
  apiRef: React.RefObject<GridApi | null>,
): number[] {
  const [widths, setWidths] = useState<number[]>([]);

  useEffect(() => {
    const getWidths = () => {
      try {
        const cols = apiRef.current?.getAllColumns();
        if (!cols) return;
        setWidths(
          cols.map(
            (c: { computedWidth?: number; width?: number }) =>
              c.computedWidth ?? c.width ?? 100,
          ),
        );
      } catch {
        // apiRef ещё не готов
      }
    };

    // DataGrid монтируется раньше footer-а, поэтому к этому моменту
    // apiRef.current уже заполнен. Подписки на изменение ширин/порядка.
    getWidths();

    const api = apiRef.current;
    const unsubResize = api?.subscribeEvent?.("columnWidthChange", getWidths);
    const unsubReorder = api?.subscribeEvent?.("columnOrderChange", getWidths);

    return () => {
      unsubResize?.();
      unsubReorder?.();
    };
  }, [apiRef]);

  return widths;
}
