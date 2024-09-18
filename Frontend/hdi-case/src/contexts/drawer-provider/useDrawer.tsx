import { useContext } from "react";
import { DrawerProviderContext, IDrawerProviderProps } from "./provider";

const useDrawer = (): IDrawerProviderProps => {
  return useContext(DrawerProviderContext);
};
export default useDrawer;
