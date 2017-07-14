using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPSocketProgramming.Class
{
    class Server
    {
        private IPAddress ip;
        private int port;
        private int clientcount = 0;
        private bool running = true;

        public List<Client> clients = new List<Client>();

        public delegate void GotDataFromCTHandler(object sender, string msg);
        public event GotDataFromCTHandler GotDataFromCTC;

        public Server(IPAddress ip,int port,bool autoStart = false)
        {
            this.ip = ip;
            this.port = port;
            
                
        }

        public void Run()
        {
            new Thread(() =>
            {
                TcpListener listener = new TcpListener(this.ip, this.port);
                listener.Start();

                while(running)
                {
                    if(listener.Pending())
                    {
                        Client client = new Client(listener.AcceptTcpClient(),this.clientcount);
                        client.internalGotDataFromCTC += GotDataFromClient;
                        clients.Add(client);
                        this.clientcount++;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                Stop();
            }
            ).Start();
        }

        private void GotDataFromClient(object sender,string data)
        {
            GotDataFromCTC(sender, data);
        }

        public void SendToAll(string data)
        {
            this.clients.ForEach(client => client.Send(data));
        }

        public void Stop()
        {
            this.running = false;
            this.clients.ForEach(client => client.Close());
            clients.Clear();
        }
    }
}
