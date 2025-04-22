import React from "react";

export function Button({ onMouseDown, children, className }: { onMouseDown: (e: React.MouseEvent) => void; children: React.ReactNode; className?: string; }) {
  return (
    <button
      onMouseDown={onMouseDown}
      className={`px-2 py-1 border rounded bg-gray-100 hover:bg-gray-200 ${className}`}
    >
      {children}
    </button>
  );
}
