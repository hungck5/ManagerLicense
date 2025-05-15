import { ReactNode, useMemo, useState } from "react";
import { ColumnDef, TableContext } from "./TableContext";

interface TableProps<T> {
  data: T[];
  keyField: keyof T;
  children: ReactNode;
  emptyText?: string;
}

export function Table<T>({ data, keyField, children, emptyText = "No data" }: TableProps<T>) {
  const [columns, setColumns] = useState<ColumnDef<T>[]>([]);

  const registerColumn = (col: ColumnDef<T>) => {
      setColumns((prev) => {
      const exists = prev.some(c => c.accessor === col.accessor);
      return exists ? prev : [...prev, col];
    });
  };

  const contextValue = useMemo(() => ({ columns, registerColumn }), [columns]);

  return (
    <TableContext.Provider value={contextValue}>
      <div className="overflow-x-auto border rounded-md shadow border-gray-200">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-100">
            <tr>
              {columns.map((col, i) => (
                <th key={i} className="px-4 py-2 text-left text-sm font-semibold text-gray-700">
                  {col.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {data.length > 0 ? (
              data.map((row: T) => (
                <tr key={String(row[keyField])} className="hover:bg-gray-50 border-t border-gray-200">
                  {columns.map((col, i) => (
                    <td key={i} className="px-4 py-2 text-sm text-gray-800">
                      {col.render ? col.render(row[col.accessor as keyof T], row) : String(row[col.accessor as keyof T])}
                    </td>
                  ))}
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={columns.length} className="px-4 py-4 text-center text-gray-500">
                  {emptyText}
                </td>
              </tr>
            )}
          </tbody>
          {children}
        </table>
      </div>
    </TableContext.Provider>
  );
}
