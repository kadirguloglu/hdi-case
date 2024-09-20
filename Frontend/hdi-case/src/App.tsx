/* eslint-disable @typescript-eslint/no-explicit-any */
import { DrawerProvider } from "./contexts/drawer-provider";
import "devextreme/dist/css/dx.material.purple.light.compact.css";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { AuthenticationProvider } from "./contexts/authentication-provider";
import useAuthentication from "./contexts/authentication-provider/useAuthentication";
import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import "photoswipe/dist/photoswipe.css";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import LoadingProvider from "./contexts/loading-provider/provider";
import { CircularProgress } from "@mui/material";

import LayoutScreen from "./components/layout";
import LoginScreen from "./screens/login";
import DashboardScreen from "./screens/dashboard";
import AdminLoginDataScreen from "./screens/admin-login-data";
import { Enum_Permission } from "./types/enums/Enum_Permission";
import LoggingScreen from "./screens/logging";
import RoleScreen from "./screens/role";
import EditRoleScreen from "./screens/role/editRole";
import { NotificationProvider } from "./contexts/notification-provider";

const Root = () => {
  return (
    <>
      <LocalizationProvider dateAdapter={AdapterMoment}>
        <LoadingProvider>
          <DrawerProvider>
            <AuthenticationProvider>
              <Router />
            </AuthenticationProvider>
          </DrawerProvider>
          <ToastContainer />
        </LoadingProvider>
      </LocalizationProvider>
    </>
  );
};
export default Root;

const Router = () => {
  const { isAuth, isLoading, isAuthenticate } = useAuthentication();
  const router = createBrowserRouter([
    {
      path: !isAuth ? "*" : "/",
      element: !isAuth ? (
        <LoginScreen />
      ) : (
        <NotificationProvider>
          <LayoutScreen />
        </NotificationProvider>
      ),
      children: [
        {
          index: true,
          element: <DashboardScreen />,
        },
        (isAuthenticate(Enum_Permission.AdminLoginDataList)
          ? {
              path: "admin-login-data",
              element: <AdminLoginDataScreen />,
            }
          : false) as any,
        (isAuthenticate(Enum_Permission.LoggingList)
          ? {
              path: "logging",
              element: <LoggingScreen />,
            }
          : false) as any,
        (isAuthenticate(Enum_Permission.RoleList)
          ? {
              path: "role",
              element: <RoleScreen />,
            }
          : false) as any,
        (isAuthenticate(Enum_Permission.RoleInsert)
          ? {
              path: "editRole/:id",
              element: <EditRoleScreen />,
            }
          : false) as any,
      ].filter(Boolean),
    },
  ]);
  if (isLoading) {
    return <CircularProgress />;
  }
  return (
    <RouterProvider
      router={router as any}
      fallbackElement={<CircularProgress />}
    />
  );
};
