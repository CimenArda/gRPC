using Grpc.Net.Client;
using GrpcMessageClient;
using GrpcServer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var messageClient = new Message.MessageClient(channel);

            #region İlkDeneme
            //var greetClient = new Greeter.GreeterClient(channel);
            //HelloReply result = await greetClient.SayHelloAsync(new HelloRequest
            //{
            //    Name = "Arda'dan Selamlar",
            //});
            //Console.WriteLine(result.Message);

            #endregion
            #region Unary
            // MessageResponse response =await messageClient.SendMessageAsync(new MessageRequest
            //{
            //    Message = "Merhaba",
            //    Name = "Arda"
            //});
            //Console.WriteLine(response.Message);
            #endregion
            #region Server Streaming 
            //var response = messageClient.SendMessage(new MessageRequest
            //{
            //    Message = "Selamm",
            //    Name = "Arda",
            //});
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
            //{
            //    Console.WriteLine(response.ResponseStream.Current.Message);

            //}

            #endregion
            #region Client Streaming
            //var request = messageClient.SendMessage();
            //for (int i = 0; i < 10; i++)
            //{
            //    await Task.Delay(1000);
            // await   request.RequestStream.WriteAsync(new MessageRequest
            //    {
            //            Name ="Arda",
            //            Message = "Mesaj gönderiyorum:" +i
            //    });

            //}
            ////stream datanın sonlandıgını ifade eder
            //await request.RequestStream.CompleteAsync();
            //Console.WriteLine((await request.ResponseAsync).Message);

            #endregion

            #region Bi-directional Streaming
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var request = messageClient.SendMessage();
            var task1   = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(1000);
                    await request.RequestStream.WriteAsync(new MessageRequest { Name = "Arda", Message = "Client Mesaj" + i });
                }
            });
            while (await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
            {
                Console.WriteLine(request.ResponseStream.Current.Message);

            }
            await task1;
            await request.RequestStream.CompleteAsync();
            
            #endregion
        }
    }
}
