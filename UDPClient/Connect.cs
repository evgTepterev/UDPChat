using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace UDPClient
{
    public static class Connect
    {
        private static string ip = "25.79.252.1";
        private static int port = 1488;
        private static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        public static Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public static string ConnectToServer(byte[] bytes)
        {
            
            SendDataToServer(bytes);
            Thread.Sleep(700);

            string answer = DataBuffer.dataBuffer.LastOrDefault();
            return answer;
            
            //new Thread(delegate () { string s = ReceiveDataFromServer(); }).Start();
            
        }
        public static async Task RecieveDataAsync()
        {
            await Task.Run(() => ReceiveDataFromServer());
        }
        
        private static void ReceiveDataFromServer() 
        {
            var buffer = new byte[256];
            int size;
            var data = new StringBuilder();
            udpSocket.Connect(ipEndPoint);
            while (true)
            {
                data.Clear();
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Thread.Sleep(15000);
                do
                {
                    size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                    data.Append(Encoding.UTF8.GetString(buffer), 0, size);
                } while (udpSocket.Available > 0);
                DataBuffer.GetData(data.ToString());
            }            
        }
        private static void SendDataToServer(byte[] sendData)
        {
            udpSocket.Send(sendData);
        }
    }
}
