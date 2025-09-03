import axios from "axios";

export const baseApi = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL!,
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
  },
});
