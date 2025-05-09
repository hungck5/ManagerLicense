import { Navigate } from "react-router-dom";
import { useAuth } from "@/auth/auth-context";
import { ReactNode } from "react";
import { LOGIN } from "@/constants/router-constants";

interface PrivateRouteProps {
  children: ReactNode;
}

const PrivateRoute = ({ children }: PrivateRouteProps) => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return <div>Loading...</div>;
  }
  return isAuthenticated ? children : <Navigate to={LOGIN} replace />;
};

export default PrivateRoute;
