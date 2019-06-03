using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace KlientServer
{
    class Program
    {
        private static IPAddress GetLocalHostIP()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return Array.Find(host.AddressList, ip => ip.AddressFamily == AddressFamily.InterNetwork);

        }

        private static void MakeClient(IPAddress iPAddress, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(iPAddress, port);


            while (true)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(Console.ReadLine());
                socket.Send(bytes);
            }
        }

        private static void MakeServer(IPAddress iPAddress, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(5); //ilosc klientow


            while (true)
            {
                Console.WriteLine("eee");
                Socket cli = socket.Accept();
                
                Console.WriteLine($"Połączono z {cli.RemoteEndPoint}");

                byte[] bytes = new byte[10];
                cli.Receive(bytes);

                String msg = System.Text.Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"Odebrano: {msg}");
                cli.Send(Encoding.ASCII.GetBytes(msg));
                
            }
        }

        static void Main(string[] args)
        {
            int port = 9000;

            MakeServer(GetLocalHostIP(), port);
            //MakeClient(GetLocalHostIP(), port);
        }
    }
}