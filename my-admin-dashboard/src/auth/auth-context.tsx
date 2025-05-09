import { User } from "oidc-client-ts";
import { createContext, ReactNode, useContext, useEffect, useState } from "react";
import { userManager } from "@/auth/userManager";

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  login: () => void;
  logout: () => void;
  signinRedirectCallback: () => Promise<void>;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const login = () => {
    userManager.signinRedirect();
  };

  const logout = () => {
    userManager.signoutRedirect();
  };

  const signinRedirectCallback = async () => {
    const user = await userManager.signinRedirectCallback();
    setUser(user);
  };

  useEffect(() => {
    
    userManager.getUser().then((loadedUser) => {
      
      if (loadedUser && !loadedUser.expired) {
        setUser(loadedUser);
      } else {
        setUser(null);
      }
      setIsLoading(false);
    });
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user && !user.expired,
        login,
        logout,
        signinRedirectCallback,
        isLoading,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};