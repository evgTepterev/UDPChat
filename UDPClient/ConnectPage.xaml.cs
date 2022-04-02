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
            string dataFromClient = $"{nickname}:{password}";
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, dataFromClient);
            Connect.ConnectToServer(mStream.ToArray());
        }
        private void BTNToRegistration_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RegistrationPage());
        }
    }
}
