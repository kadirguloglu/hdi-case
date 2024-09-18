import React from "react";
import { Enum_Permission } from "../../types/enums/Enum_Permission";
import useAuthentication from "./useAuthentication";

interface IsAuthenticationProps {
  permission: Enum_Permission | Enum_Permission[];
  children: React.ReactNode;
}
const IsAuthentication: React.FC<IsAuthenticationProps> = ({
  permission,
  children,
}) => {
  const { isAuthenticate } = useAuthentication();
  if (isAuthenticate(permission)) {
    return children;
  }
  return null;
};

export default IsAuthentication;
