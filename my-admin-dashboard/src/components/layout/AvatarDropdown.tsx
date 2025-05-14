import { useState, useRef, useEffect } from "react";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/avatar/index";
import {
  User,
  Settings,
  LogOut,
} from "lucide-react";
import { useAuth } from "@/auth/auth-context";

export function AvatarDropdown() {
  const [open, setOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);
  const {logout, user} = useAuth();
console.log(user?.profile);
console.log(user);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className="relative ml-4" ref={menuRef}>
      
      <button onClick={() => setOpen(!open)}>
        <Avatar>
          <AvatarImage src="https://github.com/shadcn.png" alt="User" />
          <AvatarFallback>U</AvatarFallback>
        </Avatar>
      </button>

      {/* Dropdown */}
      <div
        className={`absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg z-20 transition-all duration-200 origin-top-right border border-gray-200
        ${open ? "scale-100 opacity-100" : "scale-95 opacity-0 pointer-events-none"}`}
      >
        <div className="px-4 py-3 border-b border-black/10">
          <p className="text-sm font-medium text-gray-800">John Doe</p>
          <p className="text-xs text-gray-500">johndoe@email.com</p>
        </div>
        <ul className="py-1">
          <li>
            <button className="flex items-center gap-3 w-full text-left px-4 py-2 text-sm hover:bg-gray-100">
              <User size={16} /> Profile</button>
          </li>
          <li>
            <button className="flex items-center gap-3 w-full text-left px-4 py-2 text-sm hover:bg-gray-100">
              <Settings size={16} /> Settings</button>
          </li>
          <li>
            <button className="flex items-center gap-3 w-full text-left px-4 py-2 text-sm hover:bg-gray-100"
              onClick={logout}>
              <LogOut size={16} /> Logout</button>
          </li>
        </ul>
      </div>
    </div>
  );
}
