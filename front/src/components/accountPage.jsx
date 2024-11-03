import React, { useEffect, useState } from 'react';
import { Layout, List, Typography, Button, message, Empty, Modal } from 'antd';
import api from './api';
import AccountCard from './accountCard';

const { Content } = Layout;
const { Title } = Typography;

function UserHomePage() {
  const [accounts, setAccounts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [creatingAccount, setCreatingAccount] = useState(false);

  const fetchUserAccounts = async () => {
    setLoading(true);
    try {
      const response = await api.get('/Accounts');
      if (response.data.isSuccess) {
        setAccounts(response.data.data || []);
      } else {
        message.error('Не удалось загрузить аккаунты: ' + response.data.errors.join(', '));
      }
    } catch (error) {
      console.error('Ошибка при загрузке аккаунтов:', error);
      message.error('Произошла ошибка при загрузке аккаунтов: ' + (error.response?.data.errors?.join(', ') || error.message));
    } finally {
      setLoading(false);
    }
  };

  const createAccount = async () => {
    try {
      const response = await api.post('/CreateAccount');
      if (response.data.isSuccess) {
        message.success('Счет успешно создан!');
        setAccounts((prevAccounts) => [...prevAccounts, response.data.data]);
        setCreatingAccount(false); // Закрываем модальное окно
      } else {
        message.error('Не удалось создать счет: ' + response.data.errors.join(', '));
      }
    } catch (error) {
      console.error('Ошибка при создании счета:', error);
      message.error('Произошла ошибка при создании счета: ' + (error.response?.data.errors?.join(', ') || error.message));
    }
  };

  const handleDeposit = (accountNumber) => {
    // Логика пополнения счета
    message.info(`Пополнить счет номер: ${accountNumber}`);
  };

  const handleTransfer = (accountNumber) => {
    // Логика перевода средств
    message.info(`Перевести средства со счета номер: ${accountNumber}`);
  };

  useEffect(() => {
    fetchUserAccounts();
  }, []);

  return (
    <Layout style={{ minHeight: '100vh', backgroundColor: '#f0f2f5' }}>
      <Content style={{ padding: '24px' }}>
        <Title level={2}>Ваши аккаунты</Title>
        
        {/* Кнопка для создания нового счета */}
        <Button type="primary" style={{ marginBottom: '16px' }} onClick={() => setCreatingAccount(true)}>
          Создать новый счет
        </Button>

        {loading ? (
          <div>Загрузка...</div>
        ) : accounts.length > 0 ? (
          accounts.map(account => (
            <AccountCard 
              key={account.number} 
              account={account} 
              onDeposit={handleDeposit} 
              onTransfer={handleTransfer} 
            />
          ))
        ) : (
          <Empty 
            description={
              <span>
                У вас нет счетов. <br />
                <Button type="primary" style={{ marginTop: '16px' }} onClick={() => setCreatingAccount(true)}>
                  Создать счет
                </Button>
              </span>
            }
          />
        )}

        <Button type="primary" style={{ marginTop: '16px' }} onClick={() => window.location.replace('/logout')}>
          Выйти
        </Button>

        <Modal
          title="Создать счет"
          open={creatingAccount}
          onOk={createAccount}
          onCancel={() => setCreatingAccount(false)}
        >
          <p>Вы уверены, что хотите создать новый счет?</p>
        </Modal>

      </Content>
    </Layout>
  );
}

export default UserHomePage;
