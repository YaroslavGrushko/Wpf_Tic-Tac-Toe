using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_Tic_Tac_Toe
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// клиентское игровое окно:
    public partial class Window1 : Window
    {
        private MainWindow Owner;
        private bool isServer;
        private bool isMyTurn = false;
        private int[][] board = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };// 0=netral, 1=server, 2=clint
        private SocketManagement con;
        private string[] mapping = { "", "X", "O" };// 0=netral, 1=server, 2=clint
        private bool isFinished = false;
        private bool isWinner = false;
        //part of the network protocol on the client:
        //static TcpClient client;
        //static NetworkStream stream;
        //--------------------------------------------
        public Window1(MainWindow owner, bool isServer, SocketManagement con)
        {
            Loaded += Window1_MyLoaded;
            this.isMyTurn = isServer;
            this.Owner = owner;
            this.isServer = isServer;
            //new ResizeForBorderlessForm(this) { AllowResizeAll = false, AllowMove = true };
            InitializeComponent();
            this.con = con;
        }
        private void ReSetBoard()
        {
            p00.Content = mapping[board[0][0]];
            p01.Content = mapping[board[0][1]];
            p02.Content = mapping[board[0][2]];
            p10.Content = mapping[board[1][0]];
            p11.Content = mapping[board[1][1]];
            p12.Content = mapping[board[1][2]];
            p20.Content = mapping[board[2][0]];
            p21.Content = mapping[board[2][1]];
            p22.Content = mapping[board[2][2]];
        }

        private void CheckBoard()
        {
            // if (!this.InvokeRequired)
            if (this.Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                // V Check
                if (board[0][0] != 0 && board[0][1] != 0 && board[0][1] != 0 && board[0][0] == board[0][1] && board[0][1] == board[0][2] && board[0][0] == board[0][2])
                {
                    // V0
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) isWinner = true;
                }
                else if (board[1][0] != 0 && board[1][1] != 0 && board[1][1] != 0 && board[1][0] == board[1][1] && board[1][1] == board[1][2] && board[1][0] == board[1][2])
                {
                    // V1
                    isFinished = true;
                    if ((isServer && board[1][0] == 1) || (!isServer && board[1][0] == 2)) isWinner = true;
                }
                else if (board[2][0] != 0 && board[2][1] != 0 && board[2][1] != 0 && board[2][0] == board[2][1] && board[2][1] == board[2][2] && board[2][0] == board[2][2])
                {
                    // V2
                    isFinished = true;
                    if ((isServer && board[2][0] == 1) || (!isServer && board[2][0] == 2)) isWinner = true;
                }
                // H Check
                else if (board[0][0] != 0 && board[1][0] != 0 && board[2][0] != 0 && board[0][0] == board[1][0] && board[1][0] == board[2][0] && board[0][0] == board[2][0])
                {
                    // H0
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) isWinner = true;
                }
                else if (board[0][1] != 0 && board[1][1] != 0 && board[2][1] != 0 && board[0][1] == board[1][1] && board[1][1] == board[2][1] && board[0][1] == board[2][1])
                {
                    // H1
                    isFinished = true;
                    if ((isServer && board[0][1] == 1) || (!isServer && board[0][1] == 2)) isWinner = true;
                }
                else if (board[0][2] != 0 && board[1][2] != 0 && board[2][2] != 0 && board[0][2] == board[1][2] && board[1][2] == board[2][2] && board[0][2] == board[2][2])
                {
                    // H2
                    isFinished = true;
                    if ((isServer && board[0][2] == 1) || (!isServer && board[0][2] == 2)) isWinner = true;
                }
                // D Check
                else if (board[0][0] != 0 && board[1][1] != 0 && board[2][2] != 0 && board[0][0] == board[1][1] && board[1][1] == board[2][2] && board[0][0] == board[2][2])
                {
                    // D->
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) isWinner = true;
                }
                else if (board[0][2] != 0 && board[1][1] != 0 && board[2][0] != 0 && board[2][0] == board[1][1] && board[1][1] == board[0][2] && board[2][0] == board[0][2])
                {
                    // D<-
                    isFinished = true;
                    if ((isServer && board[1][1] == 1) || (!isServer && board[1][1] == 2)) isWinner = true;
                }
                if (isFinished)
                {
                    SetEnabled(true);
                    isMyTurn = false;
                    //panel1.Hide();
                    //if (isWinner) MessageBox.Show(null, "You Win!!", "Result Screen");
                    //else MessageBox.Show(null, "You Lose!!", "Result Screen");
                    if (isWinner) MessageBox.Show("You Win!!");
                    else MessageBox.Show("You Lose!!");
                    Environment.Exit(0);
                }
            }
            //else this.Invoke((MethodInvoker)delegate { CheckBoard(); });
           else this.Dispatcher.BeginInvoke(new Action(delegate
            {
                // Do your work
                CheckBoard();
            }));
        }

        private void CheckTurn()
        {
            //if (!this.InvokeRequired)
            if (this.Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                if (isMyTurn && isFinished == false)
                {
                    //panel1.Hide();
                    SetEnabled(true);
                }
                else
                {
                    //panel1.Show();
                    SetEnabled(false);
                    GetDataFromOthers();
                }
                ReSetBoard();
            }
            //else this.Invoke((MethodInvoker)delegate { CheckTurn(); });
            else this.Dispatcher.BeginInvoke(new Action(delegate
                 {
                     // Do your work
                     CheckTurn();
                 }));
        }

        private void SetEnabled(bool value)
        {
            p00.IsEnabled = value;
            p01.IsEnabled = value;
            p02.IsEnabled = value;
            p10.IsEnabled = value;
            p11.IsEnabled = value;
            p12.IsEnabled = value;
            p20.IsEnabled = value;
            p21.IsEnabled = value;
            p22.IsEnabled = value;
        }
        private void Window1_MyLoaded(object sender, RoutedEventArgs e)
        {
            //ENTRY INTO APPLICATION:
            // do work here   
            // запускаем новый поток для получения данных
            //может быть конфликт с потоком НИЖЕ
            Thread receiveThread = new Thread(new ThreadStart(CheckTurn));
            receiveThread.Start(); //старт потока
            //SendMessage();
            //CheckTurn();
        }
        private void GetDataFromOthers()
        {

            //ClientObject CO = new ClientObject(SocketManagement._CLIENT, null/*SocketManagement.server*/, false, 0);//isMain=false, 
            //and currentCLientID=0, because In this case, we do not need these parameters
            Task.Factory.StartNew(() => {//свой ПОТОК, может быть конфликт с потоком ВЫШЕ
                try
                {
                    AllData allData = con.getBoard(SocketManagement._CLIENT);
                    board = allData.obj;
                }
                catch (Exception) {
                    isServer = true;
                    CheckTurn();//return to to the begining
                }

                //if (board[0][0] == 1001)
                //{
                //    isServer = true;
                //    CheckTurn();//return to to the begining
                //}
                isMyTurn = true;
                CheckBoard();
                CheckTurn();
            });
        }

        private void SetBoardBasedOnButtonName(string code)
        {
            // 0=netral, 1=server, 2=clint
            char[] realCodeInChar = code.Substring(1).ToCharArray();
            board[Int32.Parse("" + realCodeInChar[0])][Int32.Parse("" + realCodeInChar[1])] = isServer ? 1 : 2;
        }

        private void p_click(object sender, EventArgs e)
        {
            if (isMyTurn && isFinished == false)
            {
                if ((String)((Button)sender).Content == "")
                {
                    SetBoardBasedOnButtonName(((Button)sender).Name);
                    ClientObject CO = new ClientObject(SocketManagement._CLIENT, SocketManagement.server, false, 0);//isMain=false, 
                    //and currentCLientID=0, because In this case, we do not need these parameters
                    con.sendBoard(board, CO);
                    isMyTurn = false;
                    CheckBoard();
                    CheckTurn();
                }
                else MessageBox.Show("Please select another box");
            }
        }
        void Window1_Closing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            Environment.Exit(0);

        }
    }
}
