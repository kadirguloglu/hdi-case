/* eslint-disable @typescript-eslint/no-explicit-any */
import * as signalR from "@microsoft/signalr";
import React, {
  createContext,
  useCallback,
  useEffect,
  useRef,
  useState,
} from "react";
import { Company } from "../../types/entities/Company";
import { Aggrement } from "../../types/entities/aggrements/Aggrement";

interface Notifications {
  company: Company;
  aggrement: Aggrement;
}
export interface INotificationContextProps {
  notifications: Notifications[] | null;
  connection: signalR.HubConnection | null;
}

const DefaultNotificationContextContext: INotificationContextProps = {
  notifications: [],
  connection: null,
};

export const NotificationContext = createContext(
  DefaultNotificationContextContext
);

const NotificationProvider = ({ children }: { children: React.ReactNode }) => {
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const oneTimeCheckRef = useRef<boolean>(false);
  const [notifications, setNotifications] = useState<Notifications[] | null>(
    null
  );

  // Socket Connections
  const connectSocket = useCallback(async () => {
    const token: string | null = localStorage.getItem("bearerToken");
    if (
      oneTimeCheckRef.current ||
      connectionRef.current?.state === "Connected" ||
      !token
    ) {
      return;
    }
    try {
      // setIsLoading(true);
      oneTimeCheckRef.current = true;

      const retryTimes = [0, 3000, 10000, 60000];
      // setGroup(groupResult?.data ?? null);
      const connection = new signalR.HubConnectionBuilder()
        .withUrl(
          `${import.meta.env.VITE_API_ODATA_URL}/api/hubs/Notification`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
            accessTokenFactory: () => token,
            logger: signalR.LogLevel.Debug,
          }
        )
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: (context) => {
            const index =
              context.previousRetryCount < retryTimes.length
                ? context.previousRetryCount
                : retryTimes.length - 1;
            return retryTimes[index];
          },
        })
        .build();
      //   connection.serverTimeoutInMilliseconds = 60 * 1000 * 1000;
      // connection.keepAliveIntervalInMilliseconds = 15 * 1000 * 2;

      await connection.start();

      connection.on(`NewAggrementNotification`, (data: Notifications) => {
        setNotifications((x) => [data, ...(x ?? [])]);
      });

      connectionRef.current = connection;
    } catch {
      oneTimeCheckRef.current = false;
      setTimeout(() => {
        connectSocket();
      }, 1000);
    }
  }, [connectionRef]);

  useEffect(() => {
    connectSocket();
    return () => {};
  }, [connectSocket]);

  const values = {
    connectSocket,
    notifications,
    connection: connectionRef.current,
  };

  return (
    <NotificationContext.Provider value={values}>
      {children}
    </NotificationContext.Provider>
  );
};
export default NotificationProvider;
