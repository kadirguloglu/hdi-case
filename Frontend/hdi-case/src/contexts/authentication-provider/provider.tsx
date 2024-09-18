import React, { createContext, useEffect, useState } from "react";
import { AdminLoginData } from "../../types/adminLoginData";
import { login } from "../../business/actions/adminLoginData";
import { useInterval } from "ahooks";
import { getCurrentUser } from "../../business/actions/auhentication";
import { RouteObject } from "react-router-dom";
import { CircularProgress } from "@mui/material";
import { Enum_Permission } from "../../types/enums/Enum_Permission";

export interface IAuthenticationProviderProps {
  isAuth: boolean;
  permissionKeys: number[];
  user: AdminLoginData | null;
  signIn: (email: string, password: string) => Promise<boolean>;
  signOut: () => void;
  isDeveloper: boolean;
  isAuthenticate: (permission: Enum_Permission | Enum_Permission[]) => boolean;
  isLoading: boolean;
}

const DefaultAuthenticationProviderContext: IAuthenticationProviderProps = {
  isAuth: false,
  permissionKeys: [],
  user: null,
  signIn: async () => {
    return false;
  },
  signOut: () => {},
  isDeveloper: false,
  isAuthenticate: () => false,
  isLoading: true,
};

export const AuthenticationProviderContext = createContext(
  DefaultAuthenticationProviderContext
);

export type NewRouteObject = RouteObject & {
  name?: string | null | undefined;
  children?: NewRouteObject[];
};

const AuthenticationProvider = ({
  children,
}: {
  children: React.ReactNode;
}) => {
  const [isAuth, setIsAuth] = useState<boolean>(false);
  const [permissionKeys, setPermissionKeys] = useState<number[]>([]);
  const [user, setUser] = useState<AdminLoginData | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const isDeveloper = user?.isDeveloper ?? false;
  useEffect(() => {
    const initPage = async () => {
      const token: string | null = localStorage.getItem("bearerToken");
      if (token && token.length > 0) {
        setIsAuth(true);
        await getPermissions();
      } else {
        setIsAuth(false);
      }
      setTimeout(() => {
        setIsLoading(false);
      }, 500);
    };
    initPage();
  }, []);

  useInterval(() => {
    getPermissions();
  }, 5 * 60 * 1000);

  const getPermissions = async () => {
    try {
      const x = await getCurrentUser();
      if (x.data) {
        const { data, isSuccessfull } = x;
        if (isSuccessfull && data?.adminLoginData && data?.roles) {
          setUser(data.adminLoginData);
          const roles: number[] = [];
          for (let i = 0; i < data.roles.length; i++) {
            const element = data.roles[i];
            for (let k = 0; k < element.permissionKeys.length; k++) {
              const element1 = element.permissionKeys[k];
              roles.push(element1);
            }
          }
          setPermissionKeys(roles);
          return;
        }
      } else {
        signOut();
      }
    } catch {
      signOut();
    }
  };

  const signIn = async (email: string, password: string) => {
    try {
      const result = await login({ email: email, password: password });
      if (result?.data?.isSuccessfull && result?.data?.data) {
        localStorage.setItem("bearerToken", result.data.data.bearerToken ?? "");
        localStorage.setItem("userId", result.data.data.userId ?? "");
        getPermissions();
        setIsAuth(true);
        return true;
      } else {
        return false;
      }
    } catch {
      return false;
    }
  };
  const signOut = () => {
    localStorage.removeItem("bearerToken");
    localStorage.removeItem("userId");
    setIsAuth(false);
  };

  const isAuthenticate = (permission: Enum_Permission | Enum_Permission[]) => {
    if (isDeveloper) return true;
    if (Array.isArray(permission)) {
      let allowedPerms = 0;
      for (let i = 0; i < permission.length; i++) {
        const element = permission[i];
        if (permissionKeys.some((x) => x === Number(element))) {
          allowedPerms++;
        }
      }
      if (allowedPerms > 0) {
        return true;
      }
    } else if (!Array.isArray(permission)) {
      if (permissionKeys.some((x) => x === Number(permission))) {
        return true;
      }
    }
    return false;
  };

  const values: IAuthenticationProviderProps = {
    isAuth: isAuth,
    permissionKeys: permissionKeys,
    user: user,
    signIn: signIn,
    signOut: signOut,
    isDeveloper: isDeveloper,
    isAuthenticate: isAuthenticate,
    isLoading: isLoading,
  };

  console.log("isLoading", isLoading);

  return (
    <AuthenticationProviderContext.Provider value={values}>
      {isLoading ? <CircularProgress /> : children}
    </AuthenticationProviderContext.Provider>
  );
};
export default AuthenticationProvider;
