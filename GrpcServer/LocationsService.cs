using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc;
using Grpc.Core;
using Messages;

namespace GrpcServer
{
    public class LocationsService : Messages.InvoiceService.InvoiceServiceBase
    {
        private readonly Repository repository;
        public LocationsService()
        {
            repository = new Repository();
        }

        public override async Task GetAll(GetAllRequest request, IServerStreamWriter<GetAllResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("GetAll method invoked. Sending all known locations as a stream");

            foreach (var item in repository.locationsCollection)
            {
                await responseStream.WriteAsync(new GetAllResponse { Location = item });
            }

            Console.WriteLine("Finished reponse stream");
        }
    }
}
