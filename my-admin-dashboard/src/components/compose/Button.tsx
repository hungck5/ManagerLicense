import type { ButtonHTMLAttributes } from "react";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & { active?: boolean };

export function Button({ active = false, className = "", children, ...props }: ButtonProps) {
  return (
    <button
      type="button"
      aria-pressed={active}
      {...props}
      className={`inline-flex h-9 min-w-9 items-center justify-center rounded-md px-2 text-sm transition-colors focus-visible:outline-2 focus-visible:outline-blue-500 disabled:cursor-not-allowed disabled:opacity-40 ${active ? "bg-blue-100 text-blue-700" : "text-gray-600 hover:bg-gray-100"} ${className}`}
    >
      {children}
    </button>
  );
}
