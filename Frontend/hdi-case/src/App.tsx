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
  const { isAuth, isLoading } = useAuthentication();
  const router = createBrowserRouter([
    {
      path: !isAuth ? "*" : "/",
      element: !isAuth ? <LoginScreen /> : <LayoutScreen />,
      children: [
        {
          index: true,
          element: <DashboardScreen />,
        },
      ].filter(Boolean),
    },
  ]);
  if (isLoading) {
    return <CircularProgress />;
  }
  return (
    <RouterProvider
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      router={router as any}
      fallbackElement={<CircularProgress />}
    />
  );
};
