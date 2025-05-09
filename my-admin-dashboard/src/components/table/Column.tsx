
import { useEffect } from "react";
import { ColumnDef, useTableContext } from "./tableContext";

export function Column<T>(props: ColumnDef<T>) {
  const { registerColumn } = useTableContext<T>();

  useEffect(() => {
    registerColumn(props);
  }, []);

  return null;
}
