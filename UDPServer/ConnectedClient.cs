using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
    public class ConnectedClient
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public EndPoint EndPoint { get; set; }
        public ConnectedClient(string nickname, string password, string question, string answer, EndPoint endPoint)
        {
            Nickname = nickname;
            Password = password;
            Question = question;
            Answer = answer;
            EndPoint = endPoint;
        }        
        static List<ConnectedClient> clients = ConnectWaiting.clients;

        public static (string Description, int Code) CheckClientRegData(string nickname, EndPoint endPoint)
        {
            bool toRemove = false;
            ConnectedClient objToRemove = null;
            foreach (var client in clients)
            {
                if (client.EndPoint.ToString() == endPoint.ToString())
                {
                    objToRemove = client;
                    toRemove = true;
                    Console.WriteLine(endPoint + ": Попытка создать дополнительную учётку с уже зарегистрированного IP адреса\nУдаление старой учётки, попытка создания новой...");
                }
            }
            if (toRemove)
            {
                clients.Remove(objToRemove);
            }
            foreach (var client in clients)
            {
                if (client.Nickname != nickname) { }
                else
                {
                    return (endPoint + ":Никнейм уже существует", 0); // вернуть только код ошибки, попросить ввести всё заново
                }
            }
            return (endPoint + ": Проверка пройдена, переход к созданию учетки", 1); // впустить 
        }

        public static (string Description, int Code, string Nickname, string Question, string Answer) CheckClientLoginData(string nickname,
                                                                                                                      string password,
                                                                                                                     EndPoint endPoint)
        {
            bool haveIP = false;
            bool haveNick = false;
            bool havePass = false;
            foreach (var client in clients)
            {
                if (client.EndPoint.ToString() == endPoint.ToString())
                {
                    haveIP = true;
                }
            }
            foreach (var client in clients)
            {
                haveNick = client.Nickname == nickname;
                havePass = client.Password == password;

                if (haveIP == false && havePass == true && haveNick == true)
                {
                    var questAndAnswer = clients.Single(c => c.EndPoint.ToString() == endPoint.ToString());
                    return ("success no ip", 1, null, questAndAnswer.Question, questAndAnswer.Answer);
                }
                else if (haveIP == true && havePass == true && haveNick == true)
                {
                    return ("success yes ip", 2, null, null, null);
                } // впустить без проверок
                else if (haveIP == true && haveNick == false)
                {
                    var nick = clients.Single(c => c.EndPoint.ToString() == endPoint.ToString());
                    return ("no nickname yes ip", -1, nick.Nickname, null, null);
                }
                else if (haveIP == false && haveNick == false)
                {
                    return ("no nickname no ip", -2, null, null, null);
                }
                else if (haveIP == true && haveNick == true && havePass == false)
                {
                    var questAndAnswer = clients.Single(c => c.EndPoint.ToString() == endPoint.ToString());
                    return ("no password yes ip", -3, null, questAndAnswer.Question, questAndAnswer.Answer);
                }
                else if (haveIP == false && haveNick == true && havePass == false)
                {
                    return ("no password no ip", -4, null, null, null);
                }
            }
            return ("no nickname no ip", -2, null, null, null);
        }
    }
}
