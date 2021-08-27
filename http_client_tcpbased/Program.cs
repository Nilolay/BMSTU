using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace http_client_tcpbased
{
    class Program
    {
        private const int port = 8080;
        private const string server = "127.0.0.1";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nInsert REQUEST METHOD");
                String type = Console.ReadLine();
                Console.WriteLine("Insert URL to SEND A REQUEST examples(index.html smth.html)");
                String input = Console.ReadLine();
                
                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect(server, port);

                    byte[] data = new byte[256];
                    StringBuilder response = new StringBuilder();
                    NetworkStream stream = client.GetStream();

                    String req = type + " /" + input + @" HTTP/1.1
Host: localhost:8080
Connection: keep-alive
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9
Sec-Fetch-Site: none
Sec-Fetch-Mode: navigate
Sec-Fetch-User: ?1
Sec-Fetch-Dest: document
Accept-Encoding: gzip, deflate, br
Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";

                    

                    byte[] mas = Encoding.ASCII.GetBytes(req);

                    
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(req);
                    writer.Flush();
                    
                    Thread.Sleep(20);
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable); // пока данные есть в потоке

                    Console.WriteLine(response.ToString());

                    
                    // Закрываем потоки
                    stream.Close();
                    client.Close();
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                }

                Console.WriteLine("Запрос завершен...");
              
            }
        }
    }
}


