import { ButtonHTMLAttributes, forwardRef } from "react";

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "default" | "outline" | "destructive";
  size?: "sm" | "md";
}

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className = "", variant = "default", size = "md", ...props }, ref) => {
    let variantClass = "";
    if (variant === "default") {
      variantClass = "bg-blue-600 text-white hover:bg-blue-700";
    } else if (variant === "outline") {
      variantClass = "border border-gray-300 text-gray-700 hover:bg-gray-100";
    } else if (variant === "destructive") {
      variantClass = "bg-red-600 text-white hover:bg-red-700";
    }

    let sizeClass = size === "sm" ? "px-3 py-1 text-sm" : "px-4 py-2 text-sm";

    return (
      <button
        ref={ref}
        className={`rounded-md transition-colors duration-150 ${variantClass} ${sizeClass} ${className}`}
        {...props}
      />
    );
  }
);

Button.displayName = "Button";