import { useContext } from "react";
import {
  AuthenticationProviderContext,
  IAuthenticationProviderProps,
} from "./provider";

const useAuthentication = (): IAuthenticationProviderProps => {
  return useContext(AuthenticationProviderContext);
};
export default useAuthentication;
