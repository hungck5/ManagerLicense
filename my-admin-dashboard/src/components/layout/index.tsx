import { useState } from "react";
import { Outlet } from "react-router-dom";
import { Input } from "@/components/input/index";

import { Menu } from "lucide-react";
import ReactLogo from "@/assets/react.svg";
import { Sidebar } from "@/components/layout/Sidebar";
import { AvatarDropdown } from "./AvatarDropdown";
import { Bell } from "lucide-react";

export default function Layout() {
  const [isSidebarOpen, setSidebarOpen] = useState(true);

  return (
    <div className="flex h-screen bg-gray-100">
      <aside
        className={`
          ${isSidebarOpen ? "w-64" : "w-16"}
          shrink-0 bg-white p-2 shadow-md transition-[width] duration-300 ease-in-out motion-reduce:transition-none`}
      >
        <div className="mb-4 flex h-[72px] items-center gap-2 overflow-hidden border-b border-gray-200 px-2">
          <img src={ReactLogo} alt="Logo" className="h-8 w-8 shrink-0 object-contain" />
          <span className="sidebar-label text-xl font-bold" data-visible={isSidebarOpen} aria-hidden={!isSidebarOpen}>Admin</span>
        </div>
        
        <Sidebar isSidebarOpen={isSidebarOpen} />
      </aside>

      <main className="min-w-0 flex-1 flex flex-col">
        <header className="flex items-center justify-between bg-white shadow px-6 py-4">
          <button
            onClick={() => setSidebarOpen((open) => !open)}
            aria-label={isSidebarOpen ? "Collapse sidebar" : "Expand sidebar"}
            aria-expanded={isSidebarOpen}
            className="mr-4 text-gray-600 hover:text-gray-900"
          >
            <Menu />
          </button>
          <div className="flex-1 max-w-xl">
            <Input placeholder="Search..." className="w-full" />
          </div>
          <div className="ml-4 flex items-center gap-4">
            <div className="relative cursor-pointer">
              <Bell className="w-8 h-8 text-gray-700" />
              <span className="absolute -top-1 -right-2 bg-red-500 text-white text-xs font-bold px-1.5 py-0.5 rounded-full">
                3
              </span>
            </div>
            <AvatarDropdown />
          </div>
        </header>

        <div className="flex-1 p-6 overflow-auto">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
