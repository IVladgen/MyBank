import axios from 'axios';
import { useState } from 'react';
import { Form, Input, Button, message, Layout, Row, Col, Typography } from 'antd';

const { Content } = Layout;
const { Link } = Typography;

function CreateUserForm() {
  const [formData, setFormData] = useState({
    email: '',
    numberPhone: '',
    name: '',
    surname: '',
    password: '',
    confirmPassword: '',
  });

  const createUser = async (values) => {
    try {
      if (values.password !== values.confirmPassword) {
        message.error('Пароль и подтверждение пароля не совпадают');
        return;
      }

      const response = await axios.post('https://localhost:44327/Create', values);

      const { data } = response;
      if (data.statusCode !== 200 || !data.isSuccess) {
        const errorMessage = data.errors.length > 0 ? data.errors[0].message : 'Неизвестная ошибка';
        message.error(errorMessage);
        return;
      }

      // Успешный ответ
      message.success('Пользователь успешно создан!');
      window.location.replace('/login');
    } catch (error) {
      console.error('Ошибка создания пользователя:', error.message);
      message.error('Произошла ошибка при создании пользователя.');
    }
  };

  return (
    <Layout style={{ minHeight: '100vh', backgroundColor: '#f0f2f5' }}>
      <Content style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <Row style={{ width: '100%' }} justify="center">
          <Col xs={24} sm={18} md={12} lg={8}>
            <Form
              name="create_user"
              layout="vertical"
              onFinish={createUser}
              style={{ padding: '24px', border: '1px solid #d9d9d9', borderRadius: '8px', background: '#ffffff' }}
            >
              <Form.Item
                label="Email"
                name="email"
                rules={[{ required: true, message: 'Пожалуйста, введите ваш email!' },
                        { type: 'email', message: 'Введите корректный email!' }]}
              >
                <Input
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                />
              </Form.Item>

              <Form.Item
                label="Номер телефона"
                name="numberPhone"
                rules={[
                  { required: true, message: 'Пожалуйста, введите ваш номер телефона!' },
                  { pattern: /^\+7[0-9]{10}$/, message: 'Введите корректный номер телефона в формате +7XXXXXXXXXX!' } // Формат +7XXXXXXXXXX
                ]}
              >
                <Input
                  type="tel" // Установите тип поля на "tel"
                  value={formData.numberPhone} // Исправлено на numberPhone
                  onChange={(e) => setFormData({ ...formData, numberPhone: e.target.value })} // Исправлено на numberPhone
                />
              </Form.Item>

              <Form.Item
                label="Имя"
                name="name"
                rules={[{ required: true, message: 'Пожалуйста, введите ваше имя!' }]}
              >
                <Input
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                />
              </Form.Item>

              <Form.Item
                label="Фамилия"
                name="surname"
                rules={[{ required: true, message: 'Пожалуйста, введите вашу фамилию!' }]}
              >
                <Input
                  value={formData.surname}
                  onChange={(e) => setFormData({ ...formData, surname: e.target.value })}
                />
              </Form.Item>

              <Form.Item
                label="Пароль"
                name="password"
                rules={[{ required: true, message: 'Пожалуйста, введите ваш пароль!' }]}
              >
                <Input.Password
                  value={formData.password}
                  onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                />
              </Form.Item>

              <Form.Item
                label="Подтверждение пароля"
                name="confirmPassword"
                rules={[{ required: true, message: 'Пожалуйста, подтвердите ваш пароль!' }]}
              >
                <Input.Password
                  value={formData.confirmPassword}
                  onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })}
                />
              </Form.Item>

              <Form.Item>
                <Button type="primary" htmlType="submit" block>
                  Создать пользователя
                </Button>
              </Form.Item>
            </Form>
            <div style={{ textAlign: 'center', marginTop: '16px' }}>
              <span>Уже зарегистрированы? </span>
              <Link href="/login">
                Вход
              </Link>
            </div>
          </Col>
        </Row>
      </Content>
    </Layout>
  );
}

export default CreateUserForm;
