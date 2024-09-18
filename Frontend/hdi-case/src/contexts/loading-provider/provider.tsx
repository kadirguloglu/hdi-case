import React, { createContext, useState } from "react";
import { CircularProgress } from "@mui/material";

export interface ILoadingProviderProps {
  setIsLoading: React.Dispatch<React.SetStateAction<boolean>>;
}

const DefaultLoadingProviderContext: ILoadingProviderProps = {
  setIsLoading: () => {},
};

export const LoadingProviderContext = createContext(
  DefaultLoadingProviderContext
);

const LoadingProvider = ({ children }: { children: React.ReactNode }) => {
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const values: ILoadingProviderProps = {
    setIsLoading,
  };

  return (
    <LoadingProviderContext.Provider value={values}>
      {isLoading ? (
        <div
          style={{
            display: "flex",
            position: "fixed",
            zIndex: 9999,
            width: "100%",
            height: "100%",
            background: "rgb(222 222 222 / 39%)",
            justifyContent: "center",
            alignItems: "center",
            flexDirection: "column",
          }}
        >
          <CircularProgress />
          <p>Please Wait</p>
        </div>
      ) : null}
      {children}
    </LoadingProviderContext.Provider>
  );
};
export default LoadingProvider;
