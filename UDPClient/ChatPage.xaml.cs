using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace UDPClient
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        private static string ip = "25.79.252.1";
        private static int port = 1401;
        private static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        private static Socket udpSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static Socket socket = Connect.GetSocket();
        private static StringBuilder _myStringBuilder;
        public string MyText
        {
            get { return _myStringBuilder.ToString(); }
            set {  } //добавить текст сюда (сообщения)
        }
        public ChatPage()
        {
            InitializeComponent();
            _myStringBuilder.Append("ТЕКСТ");
            this.DataContext = MyText;
            socket.Connect(ipEndPoint);
            //udpSocket2.Connect(ipEndPoint);
            ChatBoxChangedAsync();
            ReceiveDataFromServerAsync();
        }
        async Task ReceiveDataFromServerAsync()
        {
            await Task.Run(() => ReceiveDataFromServer());
        }
        async Task ChatBoxChangedAsync()
        {
            await Task.Run(() => ChatBoxChanged());
        }
        private void ChatBoxChanged()
        {
            string message;
            DataBuffer.dataBuffer.Clear();
            while (true)
            {
                while (DataBuffer.dataBuffer.LastOrDefault() != null)
                {
                    message = DataBuffer.dataBuffer.Last();
                    _myStringBuilder.AppendLine(message);
                    DataBuffer.dataBuffer.Clear();
                }
            }
        }
        private static void ReceiveDataFromServer()
        {
            var buffer = new byte[256];
            int size;
            var data = new StringBuilder();
            while (true)
            {
                data.Clear();
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
                do
                {
                    try { size = udpSocket2.ReceiveFrom(buffer, ref senderEndPoint); }
                    catch { return; }
                    data.Append(Encoding.UTF8.GetString(buffer), 0, size);
                } while (udpSocket2.Available > 0);
                _myStringBuilder.Append(data.ToString());
            }
        }
        private void BTNMessageSend_Click(object sender, RoutedEventArgs e)
        {
            udpSocket2.SendTo(Encoding.UTF8.GetBytes(MessageBox.Text), ipEndPoint);
        }

        private void MessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
