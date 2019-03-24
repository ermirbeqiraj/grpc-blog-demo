using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GrpcServer
{
    public class Repository
    {
        public List<Messages.Location> locationsCollection;

        public Repository()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "data.json");
            var fileData = File.ReadAllText(filePath);
            locationsCollection = JsonConvert.DeserializeObject<List<Messages.Location>>(fileData);
        }
    }
}
