using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Greeting.DTOs;

namespace Greeting.TcpCli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int myTcpPort = 10001;
            int TcpServerPort = 10000;

            IPEndPoint remoteEndPoint = new(IPAddress.Loopback, myTcpPort);

            TcpClient tcpClient = new(remoteEndPoint);

            await tcpClient.ConnectAsync(IPAddress.Loopback, TcpServerPort);

            NetworkStream networkStream = tcpClient.GetStream();

            GreetDto greetDto = new()
            {
                FullName = "Armin Sh",
                FirstLine = "Hello, Nice to see you",
                SecondLine = "**"
            };
            string greetDtoAsJson = JsonSerializer.Serialize(greetDto);

            byte[] greetDtoAsByteArray = Encoding.UTF8.GetBytes(greetDtoAsJson);

            await networkStream.WriteAsync(greetDtoAsByteArray);

            byte[] bytes = new byte[1024];
            while (true)
            {
                if (networkStream.DataAvailable)
                {
                    int bytesRead = await networkStream.ReadAsync(bytes, 0, bytes.Length);
                    string greetBackDtoAsJson = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                    GreetBackDto greetBackDto = JsonSerializer.Deserialize<GreetBackDto>(greetBackDtoAsJson);

                    Console.WriteLine(greetBackDto.FirstLine);
                    Console.WriteLine(greetBackDto.SecondLine);
                }
                else
                {
                    continue;
                }
            }
     

            


        }
    }
}
