import { MenuGroup } from "@/components/layout/MenuGroup";
import { MenuItem } from "@/components/layout/MenuItem";
import {
  Home,
  BarChart,
  Settings,
  User,
  Folder,
  FileText,
  Package,
} from "lucide-react";

interface SidebarProps {
  isSidebarOpen: boolean;
}

export function Sidebar({ isSidebarOpen }: SidebarProps) {
  return (
    <nav role="navigation" aria-label="Main Sidebar">
        <ul className="space-y-2">
          <MenuItem to="/dashboard" label="Dashboard" icon={<Home size={20} />} isSidebarOpen={isSidebarOpen} />

          <MenuGroup label="Reports" icon={<BarChart size={20} />} isSidebarOpen={isSidebarOpen}>
            <MenuItem to="/reports/sales" label="Sales" icon={<FileText size={18} />} isSidebarOpen={isSidebarOpen} />
            <MenuItem to="/reports/users" label="Users" icon={<User size={18} />} isSidebarOpen={isSidebarOpen} />
          </MenuGroup>

          <MenuGroup label="Projects" icon={<Folder size={20} />} isSidebarOpen={isSidebarOpen}>
            <MenuItem to="/projects/active" label="Active" icon={<FileText size={18} />} isSidebarOpen={isSidebarOpen} />
            <MenuItem to="/projects/archive" label="Archived" icon={<FileText size={18} />} isSidebarOpen={isSidebarOpen} />
          </MenuGroup>
          <MenuItem to="/users" label="Users" icon={<User size={20} />} isSidebarOpen={isSidebarOpen} />
          <MenuItem to="/products" label="Products" icon={<Package size={20} />} isSidebarOpen={isSidebarOpen} />

          <MenuItem to="/settings" label="Settings" icon={<Settings size={20} />} isSidebarOpen={isSidebarOpen} />
        </ul>
      </nav>
  );
}
