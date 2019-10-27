using log4net.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using unirest_net.http;

namespace TransfermarktDataGenerator
{
    class Program
    {
        private const string URL = "https://api-football-v1.p.rapidapi.com/v2/teams/team/33";
        static Random random = new Random();
        static void Main(string[] args)
        {
            DataGenerator dataGenerator = new DataGenerator();
            //dataGenerator.generateNClubs(1000000);
            //dataGenerator.generateNAgents(1000000);
            dataGenerator.generateNPlayers(1000000);

            
            
        }
    }
}
