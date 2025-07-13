// src/services/authService.ts
import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5251/api';

export const registerUser = async (data: { email: string; password: string }) => {
  return await axios.post(`${API_URL}/auth/register`, data);  
};
