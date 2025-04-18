import React from "react";

type AvatarProps = {
  className?: string;
  children: React.ReactNode;
};

type AvatarImageProps = React.ImgHTMLAttributes<HTMLImageElement>;

type AvatarFallbackProps = {
  className?: string;
  children: React.ReactNode;
};

export function Avatar({ className = "", children }: AvatarProps) {
  return (
    <div
      className={`relative inline-block w-10 h-10 rounded-full overflow-hidden bg-gray-200 ${className}`}
    >
      {children}
    </div>
  );
}

export function AvatarImage({ className = "", ...props }: AvatarImageProps) {
  return (
    <img
      className={`w-full h-full object-cover ${className}`}
      {...props}
      onError={(e) => {
        (e.target as HTMLImageElement).style.display = "none";
      }}
    />
  );
}

export function AvatarFallback({ className = "", children }: AvatarFallbackProps) {
  return (
    <div
      className={`w-full h-full flex items-center justify-center text-sm text-white bg-gray-500 ${className}`}
    >
      {children}
    </div>
  );
}
