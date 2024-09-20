import { useContext } from "react";
import { NotificationContext, INotificationContextProps } from "./provider";

const useNotification = (): INotificationContextProps => {
  return useContext(NotificationContext);
};
export default useNotification;
