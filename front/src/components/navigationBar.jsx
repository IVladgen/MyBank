// src/components/NavigationBar.jsx
import React from 'react';
import { Link } from 'react-router-dom';
import { Layout, Menu } from 'antd';

const { Header } = Layout;

const NavigationBar = () => {
  // Определяем массив items
  const menuItems = [
    {
      key: '1',
      label: <Link to="/home">Главная</Link>,
    },
    {
      key: '2',
      label: <Link to="/profile">Профиль</Link>,
    },
    {
      key: '3',
      label: <Link to="/">Выход</Link>,
    },
  ];

  return (
    <Header>
      {/* Используем items вместо children */}
      <Menu theme="dark" mode="horizontal" items={menuItems} />
    </Header>
  );
};

export default NavigationBar;
