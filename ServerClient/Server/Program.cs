using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace Server
{
    class Program
    {      
        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Any;
            TcpListener server = new TcpListener(localAddress, 8080);

            server.Start();

            while (true)
            {
                Socket clientSocket = server.AcceptSocket();
                byte[] incomingStream = new byte[2048];
                clientSocket.Receive(incomingStream);

                string message = Encoding.ASCII.GetString(incomingStream).Trim('\0');
                var result = JsonConvert.DeserializeObject(message);
                string clientIP = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                Console.WriteLine(result);
                IPAddress clientAddress = IPAddress.Any;
                TcpClient tcpClient = new TcpClient(clientIP, 500);
                NetworkStream tcpStream = tcpClient.GetStream();
                tcpStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                tcpClient.Close();
                clientSocket.Close();
                clientSocket.Dispose();
            }
        }
    }
}
