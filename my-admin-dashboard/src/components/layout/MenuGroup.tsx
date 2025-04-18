import { ReactNode, useState, useEffect } from "react";
import { ChevronDown, ChevronRight } from "lucide-react";
import { Children, cloneElement, isValidElement } from "react";

interface MenuGroupProps {
  label: string;
  icon?: ReactNode;
  children: ReactNode;
  isSidebarOpen: boolean;
}

export function MenuGroup({ label, icon, children, isSidebarOpen }: MenuGroupProps) {
  const [open, setOpen] = useState(false);
  const [hovering, setHovering] = useState(false);
  const showSubmenu = isSidebarOpen ? open : hovering;

  useEffect(() => {
    if (!isSidebarOpen) {
      setOpen(false);
    }
  }, [isSidebarOpen]);

  return (
    <li
      className="relative group"
      onMouseEnter={() => !isSidebarOpen && setHovering(true)}
      onMouseLeave={() => !isSidebarOpen && setHovering(false)}
    >
      <button
        onClick={() => setOpen(!open)}
        className={`flex items-center w-full gap-2 px-2 py-2 rounded hover:bg-gray-200 transition-colors justify-start`}
        aria-expanded={open}
      >
        <span className="text-gray-700">{icon}</span>
        {isSidebarOpen && (
          <>
            <span className="flex-1 text-left font-medium text-gray-800">{label}</span>
            <span className={`transition-transform duration-300 
                ${open ? "rotate-180" : "rotate-0"}
                `}
            >
              <ChevronDown size={18} />
            </span>
          </>
        )}
        {!isSidebarOpen && 
          <span className="ml-auto text-gray-500">
          <ChevronRight size={18} />
          </span>
        }
      </button>

      <ul className={`overflow-hidden border border-gray-300
          ${isSidebarOpen ? "transition-all duration-300 ease-in-out pl-4 mt-1 space-y-1" : "transition-none absolute left-full top-0 mt-1 w-48 shadow-md rounded z-10 bg-white"} 
          ${showSubmenu ? "max-h-[500px] opacity-100 pointer-events-auto" : "max-h-0 opacity-0 pointer-events-none"}
        `}>
        {showSubmenu &&
          Children.map(children, (child) =>
            isValidElement(child)
              ? cloneElement(child as React.ReactElement<any>, { forceShowLabel: true })
              : child
          )}
      </ul>
    </li>
  );
}
