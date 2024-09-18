import React, { createContext, useState } from "react";

export interface IDrawerProviderProps {
  isDrawerOpen: boolean;
  toggleDrawer: () => void;
}

const DefaultDrawerProviderContext: IDrawerProviderProps = {
  isDrawerOpen: true,
  toggleDrawer: () => {},
};

export const DrawerProviderContext = createContext(
  DefaultDrawerProviderContext
);

const DrawerProvider = ({ children }: { children: React.ReactNode }) => {
  const [openDrawer, setOpenDrawer] = useState(true);

  const toggleDrawer = () => {
    setOpenDrawer((prevState) => !prevState);
  };
  const isDrawerOpen = openDrawer;
  const values = { isDrawerOpen, toggleDrawer };

  return (
    <DrawerProviderContext.Provider value={values}>
      {children}
    </DrawerProviderContext.Provider>
  );
};
export default DrawerProvider;
