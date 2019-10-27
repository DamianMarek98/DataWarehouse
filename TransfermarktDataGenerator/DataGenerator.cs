﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarktDataGenerator.Model;

namespace TransfermarktDataGenerator
{
    class DataGenerator
    {
        private List<String> names = new List<string>();
        private List<String> surnames = new List<string>();
        private List<String> countries = new List<string>();
        static Random random = new Random();
        public DataGenerator()
        {
            String baseFilePath = @"C:\Users\Public\Documents\Devart\dbForge Data Generator for SQL Server\Data Generators\";
            names = System.IO.File.ReadLines(baseFilePath+"FirstNamesMale.txt").ToList();
            surnames = System.IO.File.ReadLines(baseFilePath + "LastNames.txt").ToList();
            countries = System.IO.File.ReadLines(@"C:\Users\Deny\Desktop\Studia\V Sem\DataWarehouses\Countries.txt").ToList();
        }

        private void generateXAgents(int x)
        {
            for (int i = 0; i < x; i++) DataService.addAgent(generateAgent());     
            
        }

        private TAgent generateAgent()
        {
            TAgent agent =new TAgent();
            agent.Imię = generateRandomName();
            agent.Nazwisko = generateRandomSurname();
            return agent;
        }

        private TKlub generateKlub()
        {
            TKlub klub = new TKlub();
            klub.Kraj = generateRandomCountry();
            klub.Nazwa = generateRandomUppercaseChar() + generateString(1, 15);
            klub.Budżet = generateMoneyAmount(1, 250000000); //to 250 mln
            return klub;
        }

        private TWartoscZawodnika generateValue(int playerId, DateTime date)
        {
            TWartoscZawodnika value = new TWartoscZawodnika();
            value.WartoscRynkowa = random.Next(1, 100000000);
            value.DataWystawienia = date;
            value.ZawodnikId = playerId;
            return value;
        }

        private TZawodnik generateZawodnik(int minAgentId, int maxAgentId, int maxClubId, int minClubId)
        {
            TZawodnik zawodnik = new TZawodnik();
            zawodnik.Imię = generateRandomName();
            zawodnik.Nazwisko = generateRandomSurname();
            zawodnik.Pozycja = getRandomPlayerPosition();
            zawodnik.DataUrodzenia = generateDateTime(new DateTime(1980, 1, 1), new DateTime(2003, 1, 1));
            zawodnik.AgentId = random.Next(minAgentId, maxAgentId);
            zawodnik.KlubId = random.Next(minClubId, maxClubId);

            return zawodnik;
        }

        private DateTime generateDateTime(DateTime from, DateTime to)
        {
            int range = (to - from).Days;
            return from.AddDays(random.Next(range));
        }

        private string generateRandomName()
        {
            return names[random.Next(0, names.Count())];
        }

        private string generateRandomSurname()
        {
            return surnames[random.Next(0, surnames.Count())];
        }

        private string generateRandomCountry()
        {
            return countries[random.Next(0, countries.Count())];
        }

        private int generateMoneyAmount(int from, int to)
        {
            return random.Next(from, to);
        }

        private string generateString(int lengthFrom, int lengthTo)
        {          
            const string allowedChars = "abcdefghijkmnopqrstuvwxyz";
            int stringLength = random.Next(lengthFrom, lengthTo);
            char[] chars = new char[stringLength];
            for (int i = 0; i < stringLength; i++) chars[i] = allowedChars[random.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        private char generateRandomUppercaseChar()
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyz";
            allowedChars = allowedChars.ToUpper();
            char upperChar = allowedChars[random.Next(0, allowedChars.Length)];

            return upperChar;
        }

        private string getRandomPaymentType()
        {
            List<string> allowedPaymentTypes = new List<string>{ "czek", "przelew" };

            return allowedPaymentTypes[random.Next(0, allowedPaymentTypes.Count)];
        }

        private string getRandomPlayerPosition()
        {
            List<string> allowedPlayerPostions = new List<string> { "Bramkarz", "Obrońca", "Pomocnik", "Napastnik" };

            return allowedPlayerPostions[random.Next(0, allowedPlayerPostions.Count)];
        }

        public void generateNAgents(int n)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("Imię", typeof(string));
            table.Columns.Add("Nazwisko", typeof(string));

            for (int i = 0; i < n; i++)
            {
                var agent = generateAgent();
                table.Rows.Add(agent.Id, agent.Imię, agent.Nazwisko);
            }

            using (var bulk = new SqlBulkCopy("Server= DESKTOP-J5I9Q9P; Initial Catalog = DataWarehousesProject; integrated security=true"))
            {
                bulk.DestinationTableName = "Agent";
                bulk.WriteToServer(table);
            }
        }

        public void generateNClubs(int n)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("Nazwa", typeof(string));
            table.Columns.Add("Kraj", typeof(string));
            table.Columns.Add("Budżet", typeof(Int32));

            for(int i = 0; i < n; i++)
            {
                var club = generateKlub();
                table.Rows.Add(club.Id, club.Nazwa, club.Kraj, club.Budżet);
            }

            using(var bulk = new SqlBulkCopy("Server= DESKTOP-J5I9Q9P; Initial Catalog = DataWarehousesProject; integrated security=true"))
            {
                bulk.DestinationTableName = "Klub";
                bulk.WriteToServer(table);
            }
        }

        public void generateNPlayers(int n)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("Imię", typeof(string));
            table.Columns.Add("Nazwisko", typeof(string));
            table.Columns.Add("DataUrodzenia", typeof(DateTime));
            table.Columns.Add("Pozycja", typeof(string));
            table.Columns.Add("KlubId", typeof(Int32));
            table.Columns.Add("AgentId", typeof(Int32));

            int maxAgentId = getAgentMaxId(), minAgentId = getAgentMinId(), maxClubId = getClubMaxId(), minClubId = getClubMinId();

            for (int i = 0; i < n; i++)
            {
                var player = generateZawodnik(minAgentId, maxAgentId, maxClubId, minClubId);
                table.Rows.Add(player.Id, player.Imię, player.Nazwisko, player.DataUrodzenia, player.Pozycja, player.KlubId, player.AgentId);
                //Console.WriteLine("Added player number:" + i.ToString());
            }

            using (var bulk = new SqlBulkCopy("Server= DESKTOP-J5I9Q9P; Initial Catalog = DataWarehousesProject; integrated security=true"))
            {
                bulk.DestinationTableName = "Zawodnik";
                bulk.WriteToServer(table);
            }
        }

        public void generatePlayersValues(DateTime time)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("WartośćRynkowa", typeof(Int32));
            table.Columns.Add("DataWystawienia", typeof(DateTime));
            table.Columns.Add("ZawodnikId", typeof(Int32));

            int minPlayerId = getPlayerMinId(), maxPlayerId = getPlayerMaxId();

            for (int i = minPlayerId; i < maxPlayerId; i++)
            {
                //here to do!
                var value = generateValue(i, time);
                table.Rows.Add(value.Id, value.WartoscRynkowa, value.DataWystawienia, value.ZawodnikId);
            }

            using (var bulk = new SqlBulkCopy("Server= DESKTOP-J5I9Q9P; Initial Catalog = DataWarehousesProject; integrated security=true"))
            {
                bulk.DestinationTableName = "WartoscZawodnika";
                bulk.WriteToServer(table);
            }
        }


        private int getPlayerMaxId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var playerId = dbContext.Zawodnik.Max(x => x.Id);
                return playerId;
            }
        }

        private int getPlayerMinId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var playerId = dbContext.Zawodnik.Min(x => x.Id);
                return playerId;
            }
        }

        private int getClubMaxId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var clubId = dbContext.Klub.Max(x => x.Id);
                return clubId;
            }
        }

        private int getClubMinId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var clubId = dbContext.Klub.Min(x => x.Id);
                return clubId;
            }
        }

        private int getAgentMaxId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var agentId = dbContext.Agent.Max(x => x.Id);
                return agentId;
            }
        }

        public int getAgentMinId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var agentId = dbContext.Agent.Min(x => x.Id);
                return agentId;
            }
        }
    }
}
