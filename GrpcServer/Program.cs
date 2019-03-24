using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace GrpcServer
{
    class Program
    {
        static SslServerCredentials GetCerts()
        {
            var certsFolder = Path.Combine(Environment.CurrentDirectory, "Certs");
            var cacert = File.ReadAllText(Path.Combine(certsFolder, "ca.crt"));
            var cert = File.ReadAllText(Path.Combine(certsFolder, "server.crt"));
            var key = File.ReadAllText(Path.Combine(certsFolder, "server.key"));

            var certificateCollection = new List<KeyCertificatePair>
            {
                new KeyCertificatePair(cert, key)
            };
            var servCred = new SslServerCredentials(certificateCollection, cacert, SslClientCertificateRequestType.RequestAndRequireButDontVerify);
            return servCred;
        }

        static void Main(string[] args)
        {
            const int Port = 50050;
            const string host = "127.0.0.1";
            var servCred = GetCerts(); // read and return certificate files (SslServerCredentials object)

            var server = new Server()
            {
                Ports = { new ServerPort(host, Port, servCred) },
                Services = { Messages.InvoiceService.BindService(new LocationsService()) }
            };

            server.Start();

            Console.WriteLine($"Starting server on port: {Port}\nPress any key to stop");

            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }
}
