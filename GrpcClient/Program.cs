using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static SslCredentials GetSslCredentials()
        {
            var CERT_PATH = Path.Combine(Environment.CurrentDirectory, "Certs");
            var cacert = File.ReadAllText(Path.Combine(CERT_PATH, "ca.crt"));
            var cert = File.ReadAllText(Path.Combine(CERT_PATH, "client.crt"));
            var key = File.ReadAllText(Path.Combine(CERT_PATH, "client.key"));

            var keyPair = new KeyCertificatePair(cert, key);
            var Creds = new SslCredentials(cacert, keyPair);
            return Creds;
        }

        static async Task Main(string[] args)
        {
            const int Port = 50050;
            const string Host = "127.0.0.1";

            var creds = GetSslCredentials();

            var PcName = Environment.MachineName;
            var channelOptions = new List<ChannelOption>
                {
                    new ChannelOption(ChannelOptions.SslTargetNameOverride, PcName)
                };
            var channel = new Channel(Host, Port, creds, channelOptions);
            var client = new Messages.InvoiceService.InvoiceServiceClient(channel);

            Console.WriteLine("GrpcClient is ready to issue requests. Press any key to start");
            Console.ReadKey();

            using (var call = client.GetAll(new Messages.GetAllRequest()))
            {
                var responseStream = call.ResponseStream;
                while (await responseStream.MoveNext())
                {
                    Console.WriteLine(responseStream.Current.Location);
                }
            }

            Console.WriteLine("\n\nFinished GetAll request with server-stream demo. Press any key to close this window");
            Console.ReadKey();
        }
    }
}
