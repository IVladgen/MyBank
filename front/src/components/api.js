// src/api.js
import axios from 'axios';

// Создаем экземпляр Axios
const api = axios.create({
  baseURL: 'https://localhost:44327', // Замените на ваш URL
});

// Добавление интерсептора для обработки ответов
api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;
    
    // Проверка на статус 401 (неавторизован)
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Запрос на обновление токена
        const response = await api.post('/Auth', {}, {
          headers: { 'Content-Type': 'application/json' } // Указание типа содержимого
        });

        // Проверка наличия access token в ответе
        if (response.data && response.data.accessToken) {
          // Сохранение нового access токена
          const { accessToken } = response.data;
          localStorage.setItem('accessToken', accessToken);

          // Добавление нового токена в заголовок повторного запроса
          originalRequest.headers['Authorization'] = 'Bearer ' + accessToken;

          // Повторный запрос с новым токеном
          return api(originalRequest);
        } else {
          throw new Error('Access token not found in the response');
        }
      } catch (refreshError) {
        console.error('Refresh Token Failed', refreshError);
        // Перенаправление на страницу логина или вывести сообщение
        window.location.href = '/login';
      }
    }

    return Promise.reject(error);
  }
);

// Установка интерсептора для запросов
api.interceptors.request.use(config => {
  const accessToken = localStorage.getItem('accessToken');
  if (accessToken) {
    config.headers['Authorization'] = 'Bearer ' + accessToken;
  }
  return config;
});

export default api;
