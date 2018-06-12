using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Net.Sockets;
using System.ComponentModel;

namespace Wpf_Tic_Tac_Toe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isServer = false;//goes first = false!
        private SocketManagement con;// object for connecting 
        private TcpClient client;
        public static NetworkStream stream;
        public int bytesLength = 255;//1212;
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool checkIPandPort(string ip, string port)
        {
            //Check the ip and port is in valid format
            if (Regex.IsMatch(ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$") && Regex.IsMatch(port, "^[0-9]{1,6}$"))
            {
                string[] temp = ip.Split('.');
                foreach (string q in temp)
                {
                    try
                    {
                        if (Int32.Parse(q) > 255) return false;
                    }
                    catch (Exception) { return false; }
                }
                return true;
            }
            return false;
        }

        private void ConnectAsServer(string ip, int port)
        {
            con = new SocketManagement(ip, port, null);
            con.StartAsServer(con);
            //if (con.StartAsServer())
            //{
            // GameStart();
            //}
        }
        private void ConnectAsClient(string ip, int port)
        {
            client = new TcpClient();
            con = new SocketManagement(ip, port,client);
            if (con.StartAsClient())
            {
                if (con.amImain()) {
                    isServer = true;
                }//check if it main client
                else isServer = false;
                GameStart();//open Window1
            }
        }

        private void EnableAll()
        {
            textBox1.IsEnabled= true;
            textBox2.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
        }
        private void DisableAll()
        {
            textBox1.IsEnabled = false;
            textBox2.IsEnabled = false;
            button2.IsEnabled = false;
            button1.IsEnabled = false;
        }
        //private void amImain()
        //{
        //    stream = client.GetStream(); // получаем поток
        //    byte[] bytes = new byte[bytesLength];

        //    stream.Read(bytes, 0, bytes.Length);
        //    string temp = new ASCIIEncoding().GetString(bytes);
        //    char[] charOfTemp = temp.ToCharArray();
        //    int[][] obj = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
        //    for (int y = 0; y < 3; y++)
        //        for (int x = 0; x < 3; x++)
        //            obj[y][x] = Int32.Parse("" + charOfTemp[(y * 3) + x]);
        //    if (obj[0][0] == 1001) { isServer = true; }//main
        //    else { isServer = false; }//doesn't main
        //}

        private void GameStart()
        {
            this.Hide();
            //isServer==true=>this client begins
            new Window1(this, isServer, con).Show();
        }
        //Server:   
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            if (checkIPandPort(textBox1.Text, textBox2.Text))
            {
                isServer = true;
                ConnectAsServer(textBox1.Text, Int32.Parse(textBox2.Text));
            }
            else EnableAll();
        }
        
        //Client:
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            DisableAll();
            if (checkIPandPort(textBox1.Text, textBox2.Text))
            {
                ConnectAsClient(textBox1.Text, Int32.Parse(textBox2.Text));
            }
            else EnableAll();
        }
// MainWindow closing:
        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            Environment.Exit(0);

        }
    }
}
