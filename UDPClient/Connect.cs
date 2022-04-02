using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDPClient
{
    public static class Connect
    {
        private static string ip = "25.79.252.1";
        private static int port = 1488;
        private static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        private static Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);       
        public static void ConnectToServer(byte[] bytes)
        {
            udpSocket.Connect(ipEndPoint);
            udpSocket.Send(bytes);

            string s;
            new Thread(delegate () { s = ReceiveDataFromServer(); }).Start();
        }
        private static bool RegistrationOnServer(string nickname, string password, string question, string answer)
        {
            bool answerFromServer = true;
            return answerFromServer;
        }
        private static string ReceiveDataFromServer() // этот метод реализован через доп. поток
        {
            var buffer = new byte[256];
            int size;
            var data = new StringBuilder();            
            EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
            do
            {
                size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                data.Clear();
                data.Append(Encoding.UTF8.GetString(buffer), 0, size);
            } while (udpSocket.Available > 0);
            return data.ToString();
        }
        private static void SendDataToServer(string sendData)
        {
            udpSocket.Send(Encoding.UTF8.GetBytes(sendData));
        }
    }
}
