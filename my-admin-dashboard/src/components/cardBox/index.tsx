import { ReactNode } from "react";

interface CardBoxProps {
  title?: string;
  children: ReactNode;
  className?: string;
}

export default function CardBox({ title, children, className = "" }: CardBoxProps) {
  return (
    <div className={`bg-white rounded-xl shadow border border-gray-200 p-4 space-y-4 ${className}`}>
      {title && <h2 className="text-lg font-semibold text-gray-800">{title}</h2>}
      {children}
    </div>
  );
}
