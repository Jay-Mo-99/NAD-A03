﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int port;
        string message;
        NetworkStream stream;
        int byteCount;
        byte[] sendData;
        TcpClient client;
        public MainWindow()
        {
            InitializeComponent();
        }
        //Send Messeage to Server
        private void SendMessageToServer(string message)
        {
            try
            {
                
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                //Sending Error is Error Situation. 
                listBox_Display.Items.Add("Error: Unable to send message to server. " + ex.Message);
            }
        }

        //If the client click the connect button 
        private void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            //If the user input invalid input in txt_Port
            if(!int.TryParse(txt_Port.Text, out int port))
            {
                //Input Problem of Client is Notice situation
                //Add the listbox and Send Notice to Server
                listBox_Display.Items.Add("Notice: Port number is invalid: Please enter the number type(Ex: 8000)");
                //Send Message To Server
                SendMessageToServer("Notice: Port number from Client is invalid");
            }
            try
            {
                //If ip and port is valid, Connect with server
                client = new TcpClient(txt_IP.Text, port);
                listBox_Display.Items.Add("Info: Connection Succeed");
                SendMessageToServer("Info: Connection Succeed");
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                //If connection have problem, It is the Error situation
                //Add the listbox and Send the Error situation to Server
                listBox_Display.Items.Add("Error: Connection Failed");
                SendMessageToServer("Error: Connection Failed: " + ex.Message);

            }

        }

        
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Send the Data for server
                message = txt_Msg.Text;
                byteCount = Encoding.ASCII.GetByteCount(message);
                sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(message);
                stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                listBox_Display.Items.Add("Sent Data " + message);

            }
            //If the connection is not well about sending message, It is Error. 
            catch (System.NullReferenceException ex)
            {
                listBox_Display.Items.Add("Error: Failed to sent data");
                SendMessageToServer("Error: Failed to sent data :" + ex.Message);
            }
        }

        private void btn_Disconnect_Click(object sender, RoutedEventArgs e)
        {
            stream.Close();
            client.Close();
            listBox_Display.Items.Add("Info: Connection terminated");
            SendMessageToServer("Info: Connection terminated");
        }

    }
}
