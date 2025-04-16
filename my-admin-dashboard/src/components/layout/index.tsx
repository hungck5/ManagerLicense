import { useState } from "react";
import { Input } from "@/components/input/index"; // Adjust the import path as necessary
import { Avatar, AvatarFallback, AvatarImage } from "@/components/avatar/index";
import { Menu } from "lucide-react";

export default function Dashboard() {
  const [isSidebarOpen, setSidebarOpen] = useState(true);

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <aside
        className={`${
          isSidebarOpen ? "w-64" : "w-16"
        } bg-white shadow-md transition-all duration-300 p-4`}
      >
        <div className="text-xl font-bold mb-4">{isSidebarOpen ? "Dashboard" : "DB"}</div>
        <nav className="space-y-2">
          <a href="#" className="block px-2 py-1 rounded hover:bg-gray-200">Home</a>
          <a href="#" className="block px-2 py-1 rounded hover:bg-gray-200">Reports</a>
          <a href="#" className="block px-2 py-1 rounded hover:bg-gray-200">Settings</a>
        </nav>
      </aside>

      {/* Main Content */}
      <main className="flex-1 flex flex-col">
        {/* Top bar */}
        <header className="flex items-center justify-between bg-white shadow px-6 py-4">
          <button
            onClick={() => setSidebarOpen(!isSidebarOpen)}
            className="mr-4 text-gray-600 hover:text-gray-900"
          >
            <Menu />
          </button>
          <div className="flex-1 max-w-xl">
            <Input placeholder="Search..." className="w-full" />
          </div>
          <div className="ml-4">
            <Avatar>
              <AvatarImage src="https://github.com/shadcn.png" alt="User" />
              <AvatarFallback>U</AvatarFallback>
            </Avatar>
          </div>
        </header>

        {/* Content area */}
        <div className="flex-1 p-6 overflow-auto">
          <h1 className="text-2xl font-semibold">Welcome to your dashboard</h1>
          {/* Add your content here */}
        </div>
      </main>
    </div>
  );
}