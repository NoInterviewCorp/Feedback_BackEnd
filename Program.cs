using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Neo4j.Driver.V1;
using Neo4jClient;
namespace feedBack
{
    public class Program
    {
     
        public static void Main(string[] args)
        {
           /*   using (var greeter = new HelloWorldExample("bolt://localhost:7687", "neo4j", "Vfunny@123"))
        {
            greeter.PrintGreeting("hello, world");
        }
        */
    //    var client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "Vfunny@123");
    // client.Connect();
        CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
