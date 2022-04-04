﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace UDPClient
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage()
        {
            InitializeComponent();
            ChatBoxChangedAsync();
        }

        async Task ChatBoxChangedAsync()
        {
            await Task.Run(() => ChatBoxChanged());
        }
        private void ChatBoxChanged( )
        {
            string message;
            DataBuffer.dataBuffer.Clear();
            while (true) 
            {
                while (DataBuffer.dataBuffer.LastOrDefault() != null)
                {
                    message = DataBuffer.dataBuffer.Last();
                    WriteChatBox(message);
                    DataBuffer.dataBuffer.Clear();
                }    
             }            
        }
        private void WriteChatBox(string message)
        {
            ChatBox.AppendText(message);
        }
        private void BTNMessageSend_Click(object sender, RoutedEventArgs e)
        {
           Connect.udpSocket.Send(Encoding.UTF8.GetBytes(MessageBox.Text));
        }

        private void MessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
