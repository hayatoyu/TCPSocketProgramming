using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPSocketProgramming.Class;
using System.Net.Sockets;
using System.Net;

namespace TCPSocketProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "CTC-Server";
            Server server = new Server(IPAddress.Any, 1221);
            server.GotDataFromCTC += GotDataFromCTC;
            server.Run();
            Console.WriteLine("Server running...");

            while(true)
            {
                string input = Console.ReadLine();
                string command,param = string.Empty;
                if (input.Contains(" "))
                {
                    command = input.Split(' ')[0];
                    param = input.Substring(command.Length + 1);
                }
                else
                    command = input;

                switch(command)
                {
                    case "send":
                        Console.WriteLine("Send to all clients: {0}", param);
                        server.SendToAll(param);
                        break;
                    case "exit":
                        server.Stop();
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void GotDataFromCTC(object sender,string msg)
        {
            Console.WriteLine("Data from CTC-Server with ID {0} received : \r\n{1}", (sender as Client).id, msg);
        }
    }
}
