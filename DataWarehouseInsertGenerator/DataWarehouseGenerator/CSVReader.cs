using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVGenerator.Model;
using CsvHelper;

namespace CSVGenerator
{
    class CSVReader
    {
        private static string playerInfoFile = @"C:\Users\Deny\Desktop\Studia\V Sem\DataWarehouses\CSVGenerator\csvgenerator\CSVGenerator\bin\Debug\playerInfo.csv";

        private static string contractVariablesFile = @"C:\Users\Deny\Desktop\Studia\V Sem\DataWarehouses\CSVGenerator\csvgenerator\CSVGenerator\bin\Debug\contractVariables.csv";

        public static List<PlayerInfo> ReadPlayerInfo()
        {
            using (var reader = new StreamReader(playerInfoFile))
            using (var csvReader = new CsvReader(reader))
            {
                var playerInfos = csvReader.GetRecords<PlayerInfo>();
                return playerInfos.ToList();
            }
        }

        public static List<ContractVariables> ReadContractVariables()
        {
            using (var reader = new StreamReader(contractVariablesFile))
            using (var csvReader = new CsvReader(reader))
            {
                var contractVariables = csvReader.GetRecords<ContractVariables>();
                return contractVariables.ToList();
            }
        }

        public static List<PlayerContractFullInfo> ReadData()
        {
            var contractVariables = ReadContractVariables();
            var playerInfos = ReadPlayerInfo();

            var fullInfos = new List<PlayerContractFullInfo>();
            contractVariables.ForEach(variable =>
            {
                var playerInfo = playerInfos.Find(info => info.playerId == variable.playerId);
                fullInfos.Add(new PlayerContractFullInfo(variable.playerId, playerInfo.wereVariablesPaid, playerInfo.variablesAmount,
                    playerInfo.deadline, playerInfo.payday, variable.agentId, variable.commission, variable.salary, variable.signingDay, variable.expiryDay));
            });

            return fullInfos;
        }
    }
}
