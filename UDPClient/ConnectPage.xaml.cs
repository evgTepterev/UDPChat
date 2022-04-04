using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UDPClient
{
    /// <summary>
    /// Логика взаимодействия для ConnectPage.xaml
    /// </summary>
    public partial class ConnectPage : Page
    {
        public ConnectPage()
        {
            InitializeComponent();           
        }

        private void BTNConnect_Click(object sender, RoutedEventArgs e)
        {
            if (LogNicknameBox.Text.ToString().Length <= 3)
            {
                MessageBox.Show("Некорректный никнейм. Длина должна быть больше трёх символов");
            }
            else if (LogNicknameBox.Text.ToString().Length >= 20)
            {
                MessageBox.Show("Некорректный никнейм. Длина должна быть меньше 20 символов");
            }
            else if (LogPasswordBox.Password.ToString().Length <= 6)
            {
                MessageBox.Show("Некорректный пароль. Длина должна быть больше 6 символов");
            }
            else if (LogPasswordBox.Password.ToString().Length >= 64)
            {
                MessageBox.Show("Некорректный пароль. Длина должна быть меньше 64 символов");
            }            
            else
            {
                string nickname = LogNicknameBox.Text;
                string password = LogPasswordBox.Password;
                SendClientLoginData(nickname, password);
            }
        }
        private void SendClientLoginData(string nickname, string password)
        {
            string correctNickname = " ";
            string correctQuestion;
            string correctAnswer;

            string dataFromClient = $"{nickname}:{password}";
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, dataFromClient);
            string answerFromServer = Connect.ConnectToServer(mStream.ToArray());

            string[] answerFromServerArray = answerFromServer.Split(':');
            if(answerFromServerArray.Length == 2) // получает код и никнейм
            {
                correctNickname = answerFromServerArray[1];
            }
            else if(answerFromServerArray.Length == 4) //получает код, ник, вопрос и ответ
            {
                correctNickname = answerFromServerArray[1];
                correctQuestion = answerFromServerArray[2];
                correctAnswer =   answerFromServerArray[3];
            }
            int choice = Convert.ToInt32(answerFromServerArray[0]);
            switch (choice)
            {
                case 2:
                    Manager.MainFrame.Navigate(new ChatPage());
                    break;
                case 1:
                    MessageBox.Show("Вход с неизвестного IP, подтвердите вход, введя ответ на секретный вопрос");
                    //перейти на стр верификации и отправить туда вопрос, ответ и ник
                    break;
                case -1:
                    MessageBox.Show($"Неверный никнейм. Возможно, вы хотели войти в аккаунт {correctNickname}?");
                    break;
                case -2:
                    MessageBox.Show("Неверные входные данные. Повторите попытку или создайте новую учетную запись");
                    break;
                case -3:
                    MessageBox.Show("Неверный пароль. Если не помните пароль, нажмите на кнопку восстановления пароля");
                    //показать кнопку перейти на стр верификации и отправить туда вопрос, ответ и ник
                    break;
                case -4:
                    MessageBox.Show("Неверный пароль. Вы не можете восстановить пароль, так как вошли с неизвестного IP адреса");
                    break;
            }
        }
        private void BTNToRegistration_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RegistrationPage());
        }
    }
}
