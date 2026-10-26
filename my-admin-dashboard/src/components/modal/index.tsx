import { ReactNode } from "react";
import { X } from "lucide-react";

interface BaseModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  children: ReactNode;
  footer?: ReactNode;
  size?: "md" | "lg";
}

export default function Modal({ isOpen, onClose, title, children, footer, size = "md" }: BaseModalProps) {
  if (!isOpen) return null;

  return (
    <div
      className="fixed inset-0 flex items-center justify-center z-50"
      onClick={onClose}
    >
      <div className="absolute inset-0 bg-slate-200/30 backdrop-blur-sm" />
      <div
        className={`relative bg-white rounded-lg w-full ${size === "lg" ? "max-w-4xl" : "max-w-md"} p-6 shadow-lg`}
        onClick={(e) => e.stopPropagation()}
      >
        <button
          onClick={onClose}
          className="absolute top-2 right-2 text-red-500 hover:text-red-700"
          aria-label="Close modal"
        >
          <X size={20} />
        </button>
        {title && <h2 className="text-xl font-semibold mb-4">{title}</h2>}
        <div className="mb-4">{children}</div>
        {footer && <div className="flex justify-end gap-2">{footer}</div>}
      </div>
    </div>
  );
}
