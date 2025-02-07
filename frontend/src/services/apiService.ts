import axios, { AxiosInstance } from "axios";
// import { toast } from "react-toastify";

const apiService: AxiosInstance = axios.create({
  baseURL: import.meta.env[`VITE_REACT_API_BASE_URL_${'DEV'}`],
  headers: {
    "Content-Type": "application/json;charset=utf8mb4;",
    "Accept": "application/json;charset=utf8mb4;",
    "X-Requested-With": "XMLHttpRequest"
  }
});

apiService.interceptors.response.use(
  (response) => response,
  (error) => {
    return Promise.reject(error);
  }
);

export default apiService;