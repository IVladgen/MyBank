import React, { useEffect, useState } from 'react';
import api from './api'; // Подключение вашего API

const Profile = () => {
  const [userData, setUserData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await api.get('/GetUserData'); // Используйте ваш API
        console.log('Response from API:', response); // Лог ответа

        if (response.data.isSuccess) {
          setUserData(response.data.data);
        } else {
          throw new Error('Не удалось получить данные пользователя');
        }
      } catch (err) {
        console.error('Error fetching user data:', err);
        setError(err.message || 'Неизвестная ошибка');
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, []);

  if (loading) {
    return <div>Загрузка...</div>;
  }

  if (error) {
    return <div>Ошибка: {error}</div>;
  }

  if (!userData) {
    return <div>Не удалось загрузить данные о пользователе.</div>;
  }

  return (
    <div style={{ padding: '20px' }}>
      <h1>Профиль пользователя</h1>
      <div style={{ marginBottom: '10px' }}>
        <strong>Имя:</strong> {userData.name || 'Имя отсутствует'}
      </div>
      <div style={{ marginBottom: '10px' }}>
        <strong>Фамилия:</strong> {userData.surname || 'Фамилия отсутствует'}
      </div>
      <div style={{ marginBottom: '10px' }}>
        <strong>Электронная почта:</strong> {userData.email || 'Email отсутствует'}
      </div>
    </div>
  );
};

export default Profile;
