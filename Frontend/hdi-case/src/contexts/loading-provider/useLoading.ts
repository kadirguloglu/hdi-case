import { useContext } from "react";
import { LoadingProviderContext, ILoadingProviderProps } from "./provider";

const useLoading = (): ILoadingProviderProps => {
  return useContext(LoadingProviderContext);
};
export default useLoading;
