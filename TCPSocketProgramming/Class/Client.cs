using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCPSocketProgramming.Class
{
    class Client
    {
        private TcpClient client;
        private StreamWriter streamwriter;
        private StreamReader streamreader;

        private bool listen = true;
        public int id;

        public delegate void internalGotDataFromCTCHandler(object sender, string msg);
        public event internalGotDataFromCTCHandler internalGotDataFromCTC;

        public Client(TcpClient client, int id)
        {
            this.client = client;
            this.id = id;

            streamwriter = new StreamWriter(this.client.GetStream());
            streamreader = new StreamReader(this.client.GetStream());

            new Thread(() =>
            {
                Listen(streamreader);
            }
                ).Start();
        }

        public void Listen(StreamReader reader)
        {
            while (listen)
            {
                string input = reader.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    input = "Client with ID " + this.id + " disconnected";
                    internalGotDataFromCTC(this, input);
                    Close();
                    return;
                }
                internalGotDataFromCTC(this, input);
            }
        }

        public void Send(string data)
        {
            streamwriter.WriteLine(data);
            streamwriter.Flush();
        }

        public void Close()
        {
            listen = false;
            streamwriter.Close();
            client.Close();
        }
    }
}
