using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Wpf_Tic_Tac_Toe
{
    public class ServerObject
    {
        private int _PORT;
        public SocketManagement con;

        int currentClientID = 0;

        public int bytesLength = 1212;//data length


        public ServerObject(SocketManagement _CON)
        {
            // _IP = IPAddress.Parse(ip);//not needed
            _PORT = _CON._PORT;
            con = _CON;
        }
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, _PORT);

                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();//передается ли он в SocketManagment?
                    SocketManagement._CLIENT = tcpClient;//передаем tcpClient в SokManag
                    bool isMain = false;
                    if (currentClientID % 2 == 0) isMain = true;

                     ClientObject clientObject = new ClientObject(tcpClient, this, isMain, currentClientID);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                    currentClientID++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        protected internal void ISMain(AllData allData)
        {
        
        }


        protected internal void ItSelfMessage(AllData allData)//Sending message itself
        {
            
            //~New~
            //Clients are divided into pairs. First is major, second is subordinate.{(0,1),(2,3),...(2k,2k+1)}
            string destID = "";//destination ID
            int intDestID = 0;
            int.TryParse(allData.sourceID, out intDestID);
            
            //Sending message itself
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == intDestID.ToString()) // if client id equals destination client id 
                {
                    con.sendBoard(allData.obj, clients[i]); //send the data to the client with id==intDestID.ToString()
                }
            }
        }
        // трансляция сообщения подключенным клиентам

        //1. переимен. метод
        protected internal void BroadcastMessage(AllData allData)
        {
            //~Old~
            //byte[] data = Encoding.Unicode.GetBytes(message);
            //for (int i = 0; i < clients.Count; i++)
            //{
            //    if (clients[i].Id != id) // если id клиента не равно id отправляющего
            //    {
            //        clients[i].Stream.Write(data, 0, data.Length); //передача данных
            //    }
            //}
            //~New~
            //Clients are divided into pairs. First is major, second is subordinate.{(0,1),(2,3),...(2k,2k+1)}
            string destID = "";//destination ID
            int intDestID = 0;
            int.TryParse(allData.sourceID, out intDestID);
            if (allData.IsSourceMain) { intDestID++; }//client-source is chief, so id increase on +1
            else
            { intDestID--;}//id decrease 1

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == intDestID.ToString()) // if client id equals destination client id 
                {
                    con.sendBoard(allData.obj, clients[i]); //send the data to the client with id==intDestID.ToString()
                }
            }
        }

        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}