import axios, { AxiosInstance } from "axios";

const timeoutSeconds: number = 40 * 1000;

const axiosInstance: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: timeoutSeconds,
  headers: {
    "Content-Type": "application/json",
  },
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("bearerToken");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

const axiosODataInstance: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_ODATA_URL,
  timeout: timeoutSeconds,
  headers: {
    "Content-Type": "application/json",
  },
});

axiosODataInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("bearerToken");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

export { axiosInstance, axiosODataInstance };
