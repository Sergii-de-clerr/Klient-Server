using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Klient
{
    class Program
    {
        static int port = 1034;
        static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введiть кiлькiсть операцiй (цiле число не менше 1):");
                int num = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < num; i++)
                {
                    List<string> Mess = new List<string> { "Введiть операцiю:", "Введiть перший операнд:", "Введiть другий операнд:" };
                    for (int j = 0; j < 3; j++)
                    {
                        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        socket.Connect(ipPoint);
                        Console.Write(Mess[j]);
                        string operation = Console.ReadLine();
                        byte[] data = Encoding.Unicode.GetBytes(operation);
                        socket.Send(data);

                        data = new byte[256];
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;

                        do
                        {
                            bytes = socket.Receive(data, data.Length, 0);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (socket.Available > 0);
                        Console.WriteLine("ответ сервера: " + builder.ToString());

                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    Console.WriteLine("----------------------------------");
                }
                List<string> res = new List<string>();
                IPEndPoint endipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                Socket endsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                endsocket.Connect(endipPoint);
                string endoperation = "END";
                byte[] enddata = Encoding.Unicode.GetBytes(endoperation);
                endsocket.Send(enddata);
                for (int I = 0; I < 3; I++)
                {
                    enddata = new byte[256];
                    StringBuilder endbuilder = new StringBuilder();
                    int endbytes = 0;
                    do
                    {
                        endbytes = endsocket.Receive(enddata, enddata.Length, 0);
                        endbuilder.Append(Encoding.Unicode.GetString(enddata, 0, endbytes));
                        res.Add(endbuilder.ToString());
                    }
                    while (endsocket.Available > 0);
                }
                Console.WriteLine("Максимальний з операндiв: " + res[0]);
                Console.WriteLine("Мiнiмальний з операндiв: " + res[1]);
                Console.WriteLine("Середнє арифметичне операндiв: " + res[2]);
                Console.WriteLine("Кiлькiсть операцiй: " + num);
                endsocket.Shutdown(SocketShutdown.Both);
                endsocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}