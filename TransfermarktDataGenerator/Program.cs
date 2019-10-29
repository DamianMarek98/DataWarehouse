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
        static void Main(string[] args)
        {
            //CreateDataBetweenTwoTimeMoments(new DateTime(2017, 6, 30), new DateTime(2018, 1, 1), 10000); //now generating milion of records toogether in between T1 and T2 + 7500 transfers (the longest operation)
            DataGenerator dataGenerator = new DataGenerator();
            dataGenerator.generateNTransfers(1, new DateTime(2020, 6, 30));
        }

        static public void CreateDataBetweenTwoTimeMoments(DateTime T1, DateTime T2, int amount) 
        {
            DataGenerator dataGenerator = new DataGenerator();
            dataGenerator.generateNClubs(amount/1000);
            dataGenerator.generateNAgents(amount);
            dataGenerator.generateNPlayers(amount);
            dataGenerator.generatePlayersValues(T1);
            dataGenerator.generateNTransfers(1000, T1);


            //T2 moment data
            dataGenerator.generateNClubs(amount);
            dataGenerator.generateNAgents(amount);
            dataGenerator.generateNPlayers(amount);
            dataGenerator.generatePlayersValues(T2);
            dataGenerator.generateNTransfers(1000, T2);
        }
    }
}
