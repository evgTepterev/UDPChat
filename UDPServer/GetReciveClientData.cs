using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPServer
{
    public class GetReciveClientData
    {
        private static int size;
        private static byte[] buffer = new byte[256];
        private static StringBuilder receivingData = new StringBuilder();

        private static string ip = "25.79.252.1";
        private static int port = 1401;
        private static Socket udpSocket2 = null;        
        public static async Task RecieveDataAsync(EndPoint endPoint)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            udpSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket2.Bind(ipEndPoint);
            await Task.Run(() => ReceiveData(endPoint));
        }

        private static string ReceiveData(EndPoint endPoint)
        {           
            while (true)
            {
                do
                {
                    size = udpSocket2.ReceiveFrom(buffer, ref endPoint);
                    receivingData.Clear();
                    receivingData.Append(Encoding.UTF8.GetString(buffer), 0, size);
                } while (udpSocket2.Available > 0);
                SendData(receivingData.ToString());
            }
        }
        private static void SendData(string data)
        {
            foreach (var client in ConnectWaiting.clients)
            {
                udpSocket2.SendTo(Encoding.UTF8.GetBytes(data), client.EndPoint);
            }
        }
    }
}
