import { createRoot } from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import App from './App.jsx';
import CreateUserForm from './components/createUser.jsx';
import LoginUserForm from './components/loginUser.jsx';
import UserHomePage from './components/accountPage.jsx';
import Profile from './components/profile.jsx';
import NavigationBar from './components/navigationBar.jsx';
import './index.css';

// Пример простой логики аутентификации
const isAuthenticated = true; // Здесь должна быть ваша логика для проверки авторизации

const AuthenticatedLayout = () => (
  <>
    <NavigationBar />
    <Routes>
      <Route path="/home" element={<UserHomePage />} />
      <Route path="/profile" element={<Profile />} />
      {/* Можете добавить больше маршрутов */}
    </Routes>
  </>
);

createRoot(document.getElementById('root')).render(
  <BrowserRouter>
    <Routes>
      <Route path="/" element={<App />} />
      <Route path="/register" element={<CreateUserForm />} />
      <Route path="/login" element={<LoginUserForm />} />

      {/* Оберните защищенные маршруты в AuthenticatedLayout */}
      {isAuthenticated ? (
        <Route path="/*" element={<AuthenticatedLayout />} />
      ) : (
        <>
          <Route path="/home" element={<UserHomePage />} />
          <Route path="/profile" element={<Profile />} />
        </>
      )}
    </Routes>
  </BrowserRouter>
);
