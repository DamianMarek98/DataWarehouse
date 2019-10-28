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
            createDataBetweenTwoTimeMoments(new DateTime(2017, 6, 30), new DateTime(2018, 1, 30), 500000);
        }

        static public void createDataBetweenTwoTimeMoments(DateTime T1, DateTime T2, int amount) //amount of player, clubs etc generated records not less than 1000 now
        {
            DataGenerator dataGenerator = new DataGenerator();
            dataGenerator.generateNClubs(amount);
            dataGenerator.generateNAgents(amount);
            dataGenerator.generateNPlayers(amount);
            dataGenerator.generatePlayersValues(T1);
            dataGenerator.generateNTransfers(1000, T1);
            //second portind of data
            dataGenerator.generateNClubs(amount);
            dataGenerator.generateNAgents(amount);
            dataGenerator.generateNPlayers(amount);
            dataGenerator.generatePlayersValues(T2);
            dataGenerator.generateNTransfers(1000, T2);
        }
    }
}
