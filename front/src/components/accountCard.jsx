import React, { useState, useEffect } from 'react';
import { Card, Button, Modal, Input, message, List } from 'antd';
import api from './api';

const AccountCard = ({ account, currentUserId }) => {
    const [isDepositModalVisible, setIsDepositModalVisible] = useState(false);
    const [isTransferModalVisible, setIsTransferModalVisible] = useState(false);
    const [isPhoneLookupModalVisible, setIsPhoneLookupModalVisible] = useState(false);
    const [isHistoryModalVisible, setIsHistoryModalVisible] = useState(false);
    const [depositAmount, setDepositAmount] = useState('');
    const [transferAmount, setTransferAmount] = useState('');
    const [transferAccountNumber, setTransferAccountNumber] = useState('');
    const [balance, setBalance] = useState(account.balance);
    const [phoneNumber, setPhoneNumber] = useState('');
    const [associatedAccounts, setAssociatedAccounts] = useState([]);
    const [transferHistory, setTransferHistory] = useState([]);

    useEffect(() => {
        setBalance(account.balance);
    }, [account.balance]);

    const fetchTransferHistory = async () => {
        try {
            const response = await api.get(`/GetTransfersOfAccount`, {
                params: { id: account.id },
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.data.isSuccess) {
                const formattedHistory = response.data.data.map(item => {
                    const receiverName = item.isSender 
                        ? `${item.toUserName} ${item.toUserSurname ? item.toUserSurname.charAt(0) + '.' : ''}` 
                        : `${item.fromUserName} ${item.fromUserSurname ? item.fromUserSurname.charAt(0) + '.' : ''}`;

                    // Проверка на наличие суммы и имени
                    if (!item.amount || (!item.toUserName && item.isSender) || (!item.fromUserName && !item.isSender)) {
                        return null; 
                    }

                    return {
                        amount: item.amount,
                        userName: receiverName,
                        isSender: item.isSender,
                    };
                }).filter(Boolean);

                setTransferHistory(formattedHistory);
                setIsHistoryModalVisible(true);
            } else {
                message.error('Не удалось получить историю переводов.');
            }
        } catch (error) {
            message.error('Ошибка при получении истории переводов: ' + (error.response?.data.errors ? error.response.data.errors.join(', ') : error.message));
        }
    };

    const handleDeposit = async () => {
        if (!depositAmount || isNaN(depositAmount) || Number(depositAmount) <= 0) {
            message.error('Пожалуйста, введите корректную сумму для пополнения.');
            return;
        }

        try {
            const response = await api.post('/FundingAccount', {
                id: account.id,
                amount: Number(depositAmount),
            });

            if (response.data.isSuccess) {
                message.success('Счет успешно пополнен!');
                setBalance(prev => prev + Number(depositAmount));
                setDepositAmount('');
                setIsDepositModalVisible(false);
            } else {
                message.error('Не удалось пополнить счет: ' + (response.data.errors ? response.data.errors.join(', ') : 'Неизвестная ошибка'));
            }
        } catch (error) {
            message.error('Ошибка при пополнении счета: ' + (error.response?.data.errors ? error.response.data.errors.join(', ') : error.message));
        }
    };

    const handleTransfer = async () => {
        if (!transferAmount || isNaN(transferAmount) || Number(transferAmount) <= 0) {
            message.error('Пожалуйста, введите корректную сумму для перевода.');
            return;
        }

        if (!transferAccountNumber) {
            message.error('Пожалуйста, введите номер счета для перевода.');
            return;
        }

        try {
            const transferData = {
                SenderId: account.id,
                NumberReceiver: transferAccountNumber,
                Amount: parseFloat(transferAmount),
            };

            const response = await api.post('/TransferAmount', transferData);

            if (response.data.isSuccess) {
                message.success('Перевод выполнен успешно!');
                setBalance(prev => prev - parseFloat(transferAmount));
                setTransferAmount('');
                setTransferAccountNumber('');
                setIsTransferModalVisible(false);
            } else {
                message.error('Не удалось выполнить перевод: ' + (response.data.errors ? response.data.errors.join(', ') : 'Неизвестная ошибка'));
            }
        } catch (error) {
            message.error('Ошибка при выполнении перевода: ' + (error.response?.data.errors ? error.response.data.errors.join(', ') : error.message));
        }
    };

    const handlePhoneLookup = async () => {
        if (!phoneNumber) {
            message.error('Пожалуйста, введите номер телефона.');
            return;
        }

        try {
            const response = await api.get(`/GetAccountsByNumberPhone`, { params: { numberPhone: phoneNumber, IdAccount:account.id }});
            if (response.data.isSuccess) {
                setAssociatedAccounts(response.data.data);
                setIsPhoneLookupModalVisible(true);
            } else {
                message.error('Не удалось найти счета по этому номеру телефона.');
            }
        } catch (error) {
            message.error('Ошибка при поиске счетов: ' + error.message);
        }
    };

    return (
        <Card title={`Счет номер: ${account.number}`} bordered={true} style={{ marginBottom: '16px' }}>
            <p>Баланс: {balance}₽</p>
            <Button type="primary" onClick={() => setIsDepositModalVisible(true)} style={{ marginRight: '8px' }}>Пополнить</Button>
            <Button type="default" onClick={() => setIsPhoneLookupModalVisible(true)} style={{ marginRight: '8px' }}>Перевести по номеру телефона</Button>
            <Button type="default" onClick={() => setIsTransferModalVisible(true)}>Перевести по номеру счета</Button>
            <Button type="default" onClick={fetchTransferHistory}>История переводов</Button>

            {/* Модальное окно для пополнения счета */}
            <Modal title={`Пополнить счет номер: ${account.number}`} open={isDepositModalVisible} onOk={handleDeposit} onCancel={() => setIsDepositModalVisible(false)}>
                <Input type="number" placeholder="Введите сумму для пополнения" value={depositAmount} onChange={(e) => setDepositAmount(e.target.value)} />
            </Modal>

            {/* Модальное окно для поиска по номеру телефона */}
            <Modal title={`Перевод по номеру телефона`} open={isPhoneLookupModalVisible} onOk={() => setIsPhoneLookupModalVisible(false)} onCancel={() => setIsPhoneLookupModalVisible(false)}>
                <Input placeholder="Введите номер телефона" value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} />
                <Button type="primary" onClick={handlePhoneLookup} style={{ marginTop: '16px' }}>Найти счета</Button>
                <List
                    style={{ marginTop: '16px' }}
                    header={<div>Счета, связанные с номером {phoneNumber}</div>}
                    bordered
                    dataSource={associatedAccounts}
                    renderItem={(item) => (
                        <List.Item onClick={() => {
                            setTransferAccountNumber(item.number);
                            setIsPhoneLookupModalVisible(false);
                            setIsTransferModalVisible(true);
                        }} style={{ cursor: 'pointer' }}>
                            {item.number}
                        </List.Item>
                    )}
                />
            </Modal>

            {/* Модальное окно для перевода по номеру счета */}
            <Modal title={`Перевод с счета номер: ${account.number}`} open={isTransferModalVisible} onOk={handleTransfer} onCancel={() => setIsTransferModalVisible(false)}>
                <Input type="number" placeholder="Введите сумму для перевода" value={transferAmount} onChange={(e) => setTransferAmount(e.target.value)} />
                <Input placeholder="Номер счета для перевода" value={transferAccountNumber} readOnly />
            </Modal>

            {/* Модальное окно для истории переводов */}
            <Modal title="История переводов" 
                   open={isHistoryModalVisible} 
                   onOk={() => setIsHistoryModalVisible(false)} 
                   onCancel={() => setIsHistoryModalVisible(false)}>
                {transferHistory.length === 0 ? (
                    <div>Нет данных</div>
                ) : (
                    <List
                        header={<div>История переводов</div>}
                        bordered
                        dataSource={transferHistory}
                        renderItem={(item) => (
                            <List.Item>
                                <div>
                                    {item.isSender 
                                        ? `Вы отправили ${item.amount}₽ к ${item.userName}.` 
                                        : `Вы получили ${item.amount}₽ от ${item.userName}.`}
                                </div>
                            </List.Item>
                        )}
                    />
                )}
            </Modal>
        </Card>
    );
};

export default AccountCard;
