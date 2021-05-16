using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Greeting.DTOs;

namespace Greeting.TCPServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int myTcpPort = 10000;
            TcpListener tcpListener = new(IPAddress.Loopback, myTcpPort);
            tcpListener.Start();

            byte[] bytes = new byte[1024];
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                NetworkStream networkStream = tcpClient.GetStream();
             
                if (networkStream.DataAvailable)
                {
                    int bytesRead = await networkStream.ReadAsync(bytes, 0, bytes.Length);

                    string greetDtoAsJson = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                    GreetDto greetDto = JsonSerializer.Deserialize<GreetDto>(greetDtoAsJson);
                    Console.WriteLine($"Message Received From: {greetDto.FullName} {Environment.NewLine} { greetDto.FirstLine} { Environment.NewLine} { Environment.NewLine}");

                    GreetBackDto greetBackDto = new()
                    {
                        FirstLine = $"Hello Dear {greetDto.FullName}",
                        SecondLine = "We Received your message, thank you."
                    };

                    string greetBackDtoAsJson = JsonSerializer.Serialize(greetBackDto);
                    byte[] greetBackDtoAsByteArray = Encoding.UTF8.GetBytes(greetBackDtoAsJson);
                    await networkStream.WriteAsync(greetBackDtoAsByteArray);

                }
            }
       
        }
    }
}
