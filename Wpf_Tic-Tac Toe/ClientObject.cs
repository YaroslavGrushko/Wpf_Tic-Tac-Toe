using System;
using System.Net.Sockets;
using System.Text;

namespace Wpf_Tic_Tac_Toe
{
    public class ClientObject
    {
        public string Id { get; private set; }
        public static int id;
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера
        public bool isMain;//true if this client is main gamer 
        int _currentClientID;


        public int bytesLength = 1212;//data length

        public ClientObject(TcpClient tcpClient, ServerObject serverObject, bool _isMain, int currentClientID)
        {
            // Id = Guid.NewGuid().ToString();
            Id = id.ToString();
            client = tcpClient;
            Stream = client.GetStream();
            server = serverObject;
            try { serverObject.AddConnection(this); } catch (Exception) { }
            isMain = _isMain;
            _currentClientID = currentClientID;



            //notify the client or it is the main
            int[][] is_Main = new int[3][];
            is_Main[0] = new int[3];
            is_Main[1] = new int[3];
            is_Main[2] = new int[3];
            if (isMain)
            {
                is_Main[0][0] = 1;/*1001*/;
            }
            else is_Main[0][0] = 0;/*1000*/;
            //server.BroadcastMessage(new AllData(is_Main, "", false));
            //new2:
            //on first iteration isMain=true
            //server.BroadcastMessage(new AllData(is_Main, Id.ToString(), isMain));
            try { server.ItSelfMessage(new AllData(is_Main, Id.ToString(), isMain)); } catch (Exception) {
                //this method does thing if this is on server (not client)
            };

            id++;

        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                //Old Version:
                ////////////////////////
                //// получаем имя пользователя
                //string message = GetMessage();
                //userName = message;

                //message = userName + " вошел в чат";
                //// посылаем сообщение о входе в чат всем подключенным пользователям
                //server.BroadcastMessage(message, this.Id);
                ////////////////////////


                ////notify the client or it is the main
                //int[][] is_Main = new int[3][];
                //if (isMain) {
                //    is_Main[0][0] = 1001;
                //} else is_Main[0][0] = 1000;
                ////server.BroadcastMessage(new AllData(is_Main, "", false));
                ////new2:

                //server.BroadcastMessage(new AllData(is_Main, Id, false));
                AllData/*int[][]*/ message;
                             

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = server.con.getBoard(this);
                        //message = String.Format("{0}: {1}", userName, message);//~old~
                        // Console.WriteLine(message);
                        // server.BroadcastMessage(message, this.Id);//~old~
                        //~New~ 
                        
                        



                       // 1. подумать насчет id
                       // 2. переименовать метод
                        server.BroadcastMessage(message);

                    }
                    catch
                    {
                        //message = String.Format("{0}: покинул чат", userName);
                        //Console.WriteLine(message);
                        //server.BroadcastMessage(message, this.Id);

                        //--user has left the game--//
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        //private int[][] GetMessage()
        //{
        //    //Old Version:
        //    //byte[] data = new byte[64]; // буфер для получаемых данных
        //    //StringBuilder builder = new StringBuilder();
        //    //int bytes = 0;
        //    //do
        //    //{
        //    //    bytes = Stream.Read(data, 0, data.Length);
        //    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
        //    //}
        //    //while (Stream.DataAvailable);

        //    //return builder.ToString();
        //}

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}