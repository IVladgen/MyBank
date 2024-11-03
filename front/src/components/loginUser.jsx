import axios from 'axios';
import React, { useState } from 'react';
import { Form, Input, Button, message, Layout, Row, Col, Typography } from 'antd';

const { Content } = Layout;
const { Link } = Typography;

function LoginUserForm() {
  const [formData, setFormData] = useState({ email: '', password: '' });

  const loginUser = async (values) => {
    try {
      const response = await axios.post('https://localhost:44327/Login', values, {
        withCredentials: true, // Позволяет отправлять куки
      });
  
      // Проверяем структуру ответа
      if (response.data.isSuccess && response.data.data) {
        const { accessToken } = response.data.data;
        localStorage.setItem('accessToken', accessToken);
        window.location.replace('/home');
      } else {
        // Обработка ошибок
        const errorMessage = 
          response.data.errors && response.data.errors.length > 0 
            ? response.data.errors.join(', ') 
            : 'Неизвестная ошибка или вход не удался';
        message.error(errorMessage);
      }
    } catch (error) {
      console.error('Ошибка входа:', error);
      const errorMessage =
        error.response && error.response.data.message
          ? error.response.data.message
          : error.message;
      message.error('Произошла ошибка при входе в систему: ' + errorMessage);
    }
  };
  

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  };

  return (
    <Layout style={{ minHeight: '100vh', backgroundColor: '#f0f2f5' }}>
      <Content style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <Row style={{ width: '100%' }} justify="center">
          <Col xs={24} sm={18} md={12} lg={8}>
            <Form
              name="login_user"
              layout="vertical"
              onFinish={loginUser}
              style={{ padding: '24px', border: '1px solid #d9d9d9', borderRadius: '8px', background: '#ffffff' }}
            >
              <Form.Item
                label="Email"
                name="email"
                rules={[{ required: true, message: 'Пожалуйста, введите ваш email!' },
                        { type: 'email', message: 'Введите корректный email!' }]}
              >
                <Input
                  type="email"
                  value={formData.email}
                  onChange={handleChange}
                />
              </Form.Item>
              
              <Form.Item
                label="Пароль"
                name="password"
                rules={[{ required: true, message: 'Пожалуйста, введите ваш пароль!' }]}
              >
                <Input.Password
                  value={formData.password}
                  onChange={handleChange}
                />
              </Form.Item>
              
              <Form.Item>
                <Button type="primary" htmlType="submit" block>
                  Войти
                </Button>
              </Form.Item>
            </Form>
            <div style={{ textAlign: 'center', marginTop: '16px' }}>
              <span>Еще не зарегистрированы? </span>
              <Link href="/register">
                Регистрация
              </Link>
            </div>
          </Col>
        </Row>
      </Content>
    </Layout>
  );
}

export default LoginUserForm;
