using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UDPServer
{
    public class GetReciveClientData
    {
        private static int size;
        private static byte[] buffer = new byte[256];
        private static StringBuilder receivingData = new StringBuilder();
         
        public static async Task RecieveDataAsync(EndPoint endPoint)
        {
            await Task.Run(() => ReceiveData(endPoint));
        }

        private static string ReceiveData(EndPoint endPoint)
        {
            while (true)
            {
                do
                {
                    size = Program.udpSocket.ReceiveFrom(buffer, ref endPoint);
                    receivingData.Clear();
                    receivingData.Append(Encoding.UTF8.GetString(buffer), 0, size);
                } while (Program.udpSocket.Available > 0);
                return receivingData.ToString();
            }
        }
        //private void SendData(string data)
        //{
        //    EndPoint endPoint = EndPoint;
        //    Program.udpSocket.SendTo(Encoding.UTF8.GetBytes(data), endPoint);
        //}
    }
}
