import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Layout from "@/components/layout/index";
import Dashboard from "@/pages/dashboard/index";
import Settings from "@/pages/settings/index";
import LoginPage from "@/pages/Login";
import NotFound from "@/pages/NotFound";
import CallbackPage from "@/pages/CallbackPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <LoginPage />,
  },
  {
    path: "/dashboard",
    element: <Layout />,
    children: [
      {
        index: true,
        element: <Dashboard />,
      },
      {
        path: "settings",
        element: <Settings />,
      },
    ],
  },
  {
    path: "/callback",
    element: <CallbackPage />,
  },
  {
    path: "*",
    element: <NotFound />,
  },
]);

export default function AppRouter() {
  return <RouterProvider router={router} />;
}