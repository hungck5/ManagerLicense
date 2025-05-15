
import { createContext, useContext, ReactNode } from "react";

export interface ColumnDef<T> {
  header: string;
  accessor: keyof T | string;
  render?: (value: any, row: T) => ReactNode;
}

interface TableContextType<T> {
  columns: ColumnDef<T>[];
  registerColumn: (column: ColumnDef<T>) => void;
}

export const TableContext = createContext<TableContextType<any> | null>(null);

export function useTableContext<T>() {
  const ctx = useContext(TableContext);
  if (!ctx) throw new Error("useTableContext must be used within <Table>");
  return ctx as TableContextType<T>;
}
