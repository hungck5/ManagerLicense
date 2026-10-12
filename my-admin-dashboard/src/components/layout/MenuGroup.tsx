import { ReactNode, useState, useEffect } from "react";
import { ChevronDown } from "lucide-react";
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
      setHovering(false);
    }
  }, [isSidebarOpen]);

  return (
    <li
      className="relative group"
      onMouseEnter={() => !isSidebarOpen && setHovering(true)}
      onMouseLeave={() => setHovering(false)}
    >
      <button
        onClick={() => isSidebarOpen ? setOpen((expanded) => !expanded) : setHovering(true)}
        className="relative flex items-center w-full gap-2 overflow-hidden px-2 py-2 rounded hover:bg-gray-200 transition-colors justify-start"
        aria-label={label}
        title={!isSidebarOpen ? label : undefined}
        aria-expanded={showSubmenu}
      >
        <span className="flex w-5 shrink-0 items-center justify-center text-gray-700">{icon}</span>
        <span className="sidebar-label flex-1 pr-5 text-left font-medium text-gray-800" data-visible={isSidebarOpen} aria-hidden={!isSidebarOpen}>{label}</span>
        <span className={`absolute right-2 transition-[opacity,transform] duration-300 ease-in-out motion-reduce:transition-none ${isSidebarOpen ? "opacity-100" : "opacity-0"} ${open ? "rotate-180" : "rotate-0"}`} aria-hidden="true">
          <ChevronDown size={18} />
        </span>
      </button>

      <div
        className={`grid transition-[grid-template-rows,opacity] duration-300 ease-in-out motion-reduce:transition-none ${isSidebarOpen && open ? "grid-rows-[1fr] opacity-100" : "grid-rows-[0fr] opacity-0"}`}
        aria-hidden={!isSidebarOpen || !open}
        inert={!isSidebarOpen || !open}
      >
        <div className="min-h-0 overflow-hidden">
          <ul className="space-y-1 pt-1 pl-4">
            {Children.map(children, (child) =>
              isValidElement(child)
                ? cloneElement(child as React.ReactElement<any>, { forceShowLabel: true })
                : child
            )}
          </ul>
        </div>
      </div>
      {!isSidebarOpen && hovering && (
        <ul className="absolute left-full top-0 z-10 w-48 space-y-1 rounded border border-gray-300 bg-white p-1 shadow-md">
          {Children.map(children, (child) =>
            isValidElement(child)
              ? cloneElement(child as React.ReactElement<any>, { forceShowLabel: true })
              : child
          )}
        </ul>
      )}
    </li>
  );
}
