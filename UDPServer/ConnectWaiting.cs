using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using UDPClient;

namespace UDPServer
{
    static class ConnectWaiting
    {
        public static List<ConnectedClient> clients = new List<ConnectedClient>();
        public static void ConnectWaitingThread()
        {
            while (true)
            {
                var buffer = new byte[256];
                int size;
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

                do //TODO: сделать каталог куда помещать хмл файлы (мб апдату)
                {
                    Console.WriteLine("Ждём подключения клиента...");
                    size = Program.udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                    Console.WriteLine("________________________________");
                    Console.WriteLine("\tПОДКЛЮЧЕНИЕ КЛИЕНТА");                   
                    #region преобразование массива байтов в строку, а потом строку в массив подстрок
                    var mStream = new MemoryStream();
                    var binFormatter = new BinaryFormatter();
                    mStream.Write(buffer, 0, buffer.Length);
                    mStream.Position = 0;
                    var myObject = binFormatter.Deserialize(mStream) as string;
                    var tempClientData = myObject.ToString().Split(':');
                    #endregion

                    if (tempClientData.Count() == 2) // если равно двум, то клиент пытается залогиниться
                    {
                        Console.WriteLine("Информация о клиенте:");
                        Console.WriteLine("IP адрес: " + senderEndPoint);
                        Console.WriteLine("Никнейм: " + tempClientData[0]);
                        Console.WriteLine("Попытка залогиниться");
                        Console.WriteLine("________________________________");
                        var checkResult = ConnectedClient.CheckClientLoginData(tempClientData[0], tempClientData[1], senderEndPoint);
                        Console.WriteLine(checkResult.Description);
                        if (checkResult.Code == 2 || checkResult.Code == -2)
                        {
                            Program.udpSocket.SendTo(Encoding.UTF8.GetBytes(checkResult.Code.ToString()), senderEndPoint); // отправка кода клиенту
                        }
                        else if (checkResult.Code == -1)
                        {
                            string answer = checkResult.Code + ":" + checkResult.Nickname;
                            Program.udpSocket.SendTo(Encoding.UTF8.GetBytes(answer), senderEndPoint); //отправка кода и никнейма клиенту (напомнить ник, т.к. есть айпи в базе)
                        }
                        else if (checkResult.Code == -3 || checkResult.Code == 1) //TODO: ЭТО НЕБЕЗОПАСНО, В БУДУЩЕМ НАДО ПОДПРАВИТЬ, Т.К. ОТПРАВЛЯТЬ КЛИЕНТУ ОТВЕТ НА СЕКРЕТНЫЙ ВОПРОС ЧТОБ ПРОВЕРИТЬ ЭТО ТУПО!
                        {
                            string answer = checkResult.Code + ":" + checkResult.Nickname + ":" + checkResult.Question + ":" + checkResult.Answer;
                            Program.udpSocket.SendTo(Encoding.UTF8.GetBytes(answer), senderEndPoint);
                        }
                    }
                    else // если равно четырём, то пытается зарегаться
                    {
                        Console.WriteLine("Информация о клиенте:");
                        Console.WriteLine("IP адрес: " + senderEndPoint);
                        Console.WriteLine("Никнейм: " + tempClientData[0]);
                        Console.WriteLine("Попытка зарегаться");
                        Console.WriteLine("________________________________");
                        ConnectedClient.CheckClientRegData(tempClientData[0], senderEndPoint);
                        var checkResult = ConnectedClient.CheckClientRegData(tempClientData[0], senderEndPoint);
                        Console.WriteLine(checkResult.Description);
                        if(checkResult.Code == 0)
                        {
                            Program.udpSocket.SendTo(Encoding.UTF8.GetBytes(checkResult.Code.ToString()), senderEndPoint);
                        }
                        else
                        {
                            AddClientData(new ConnectedClient(tempClientData[0], tempClientData[1], tempClientData[2], tempClientData[3], senderEndPoint));
                            Program.udpSocket.SendTo(Encoding.UTF8.GetBytes(checkResult.Code.ToString()), senderEndPoint);
                        }
                    }                   
                } while (Program.udpSocket.Available > 0);
            }
        }
        static void AddClientData(ConnectedClient client)
        {
            clients.Add(client);
            Console.WriteLine("Успешно добавили клиента "+client.EndPoint+" с ником <"+client.Nickname+"> в базу");
        }
    }
}

