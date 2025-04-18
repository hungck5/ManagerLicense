import { ReactNode } from "react";
import { NavLink } from "react-router-dom";

interface MenuItemProps {
  to: string;
  icon?: ReactNode;
  label: string;
  isSidebarOpen: boolean;
  forceShowLabel?: boolean;
}

export function MenuItem({ to, icon, label, isSidebarOpen, forceShowLabel = false }: MenuItemProps) {
  const shouldShowLabel = isSidebarOpen || forceShowLabel;

  return (
    <li>
      <NavLink
        to={to}
        className={({ isActive }) =>
          `flex items-center gap-2 px-2 py-2 rounded hover:bg-gray-200 text-gray-800 justify-start
          ${isActive ? "bg-gray-100 font-semibold" : ""} 
          `
        }
      >
        {icon}
        {shouldShowLabel && <span>{label}</span>}
      </NavLink>
    </li>
  );
}
