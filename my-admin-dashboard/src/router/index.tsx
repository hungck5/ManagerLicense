import { createBrowserRouter, Navigate, RouterProvider } from "react-router-dom";
import Layout from "@/components/layout/index";
import Dashboard from "@/pages/dashboard/index";
import Settings from "@/pages/settings/index";
import LoginPage from "@/pages/Login";
import NotFound from "@/pages/NotFound";
import CallbackPage from "@/pages/CallbackPage";
import Products from "@/pages/products";
import Users from "@/pages/users";
import { AuthProvider } from "@/auth/auth-context";
import PrivateRoute from "@/router/private-router";
import { CALLBACK, DASHBOARD, HOME, LOGIN, NOT_FOUND, PRODUCTS, SETTINGS, USERS } from "@/constants/router-constants";

const router = createBrowserRouter([
  {
    path: DASHBOARD,
    element: <Navigate to={HOME} replace />,
  },
  {
    path: HOME,
    element: (
      <PrivateRoute>
        <Layout />
      </PrivateRoute>
    ),
    children: [
      {
        index: true,
        element: <Dashboard />,
      },
      {
        path: SETTINGS,
        element: <Settings />,
      },
      {
        path: PRODUCTS,
        element: <Products />,
      },
      {
        path: USERS,
        element: <Users />,
      },
    ],
  },
  {
    path: LOGIN,
    element: <LoginPage />,
  },
  {
    path: CALLBACK,
    element: <CallbackPage />,
  },
  {
    path: NOT_FOUND,
    element: <NotFound />,
  },
]);

export default function AppRouter() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  );
}