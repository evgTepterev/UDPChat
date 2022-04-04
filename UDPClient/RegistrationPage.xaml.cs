using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace UDPClient
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private void BTNRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (RegNicknameBox.Text.ToString().Length <= 3)
            {
                MessageBox.Show("Некорректный никнейм. Длина должна быть больше трёх символов");

            }
            else if (RegNicknameBox.Text.ToString().Length >= 20)
            {
                MessageBox.Show("Некорректный никнейм. Длина должна быть меньше 20 символов");

            }
            else if (RegPasswordBox.Password.ToString().Length <= 6)
            {
                MessageBox.Show("Некорректный пароль. Длина должна быть больше 6 символов");
            }
            else if (RegPasswordBox.Password.ToString().Length >= 64)
            {
                MessageBox.Show("Некорректный пароль. Длина должна быть меньше 64 символов");

            }
            else if (RegQuestionBox.Text.Length <= 10)
            {
                MessageBox.Show("Некорректный секретный вопрос. Длина должна быть больше 10 символов");

            }
            else if (RegQuestionBox.Text.Length >= 124)
            {
                MessageBox.Show("Некорректный секретный вопрос. Длина должна быть меньше 124 символов");
            }
            else if (RegAnswerBox.Text.Length >= 124)
            {
                MessageBox.Show("Некорректный ответ на вопрос. Длина должна быть меньше 124 символов");
            }
            else
            {
                string nickname = RegNicknameBox.Text;
                string password = RegPasswordBox.Password;
                string question = RegQuestionBox.Text;
                string answer = RegAnswerBox.Text;
                SendClientRegData(nickname, password, question, answer);
            }
        }
        private void SendClientRegData(string nickname, string password, string question, string answer)
        {
            string dataFromClient = $"{nickname}:{password}:{question}:{answer}";
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, dataFromClient);
            string answerFromServer = Connect.ConnectToServer(mStream.ToArray());
            switch (Convert.ToInt32(answerFromServer))
            {
                case 1:
                    Manager.MainFrame.Navigate(new ConnectPage());
                    break;
                case 0:
                    MessageBox.Show("Никнейм уже кем-то используется");
                    break;
            }
        }
        private void RegBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            while (tb.Text.Contains(":"))
            {
                tb.Text = tb.Text.Remove(tb.Text.IndexOf(':'), 1);
                tb.Select(tb.Text.LastOrDefault(), 1);
            }
        }
        private void RegBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void RegPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            while (RegPasswordBox.Password.Contains(":"))
            {
                RegPasswordBox.Password = RegPasswordBox.Password.Remove(RegPasswordBox.Password.IndexOf(':'), 1);
            }
        }
    }
}

