using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using feedBack.Services;
using feedBack.Controllers;
using feedBack;
namespace feedBack.Services
{
    
    public class GraphDbConnection : IDisposable
    {
        public GraphClient client;
        public GraphDbConnection()
        {
           // = new GraphClient(new Uri("http://localhost:7688/db/data"), "neo4j", "qwertyuiop");
          // .Connect();
            client  = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "Vfunny@123");
             client.Connect();
        }
        public void Dispose(){
            client.Dispose();
        }
    }
}