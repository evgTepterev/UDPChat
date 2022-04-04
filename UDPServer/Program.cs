using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Security;
using System.IO;

namespace UDPServer
{
    class Program
    {
        public static string ip = "25.79.252.1";
        public static int port = 1400;
        public static Socket udpSocket = null;
        static void Main()
        {

            UdpClient udpClient = new UdpClient();
            Console.WriteLine($"Текущий адрес сервера: {ip}:{port} | Желаете сменить? 1 -да, 2 - нет");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Введите айпи в формате xx.xx.xx.xx");
                    ip = Console.ReadLine();
                    Console.WriteLine("Введите порт");
                    port = Convert.ToInt32(Console.ReadLine());
                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("Неверный ответ, введите 1 или 2");
                    Main();
                    break;
            }

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(ipEndPoint);
            Console.WriteLine("Сервер запущен!");
            Thread thread = new Thread(new ThreadStart(ConnectWaiting.ConnectWaitingThread));
            thread.Start();

            RequestHandler.RequestWaitng();
        }
    }
}
