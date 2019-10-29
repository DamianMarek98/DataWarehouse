using System;
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
            String baseFilePath = @"..\..\txtFiles\";
            names = System.IO.File.ReadLines(baseFilePath+"FirstNamesMale.txt").ToList();
            surnames = System.IO.File.ReadLines(baseFilePath + "LastNames.txt").ToList();
            countries = System.IO.File.ReadLines(baseFilePath+"Countries.txt").ToList();
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

        private TWartoscZawodnika generateValueBasedOnLastOne(int playerId, DateTime date, int oldValue)
        {
            TWartoscZawodnika value = new TWartoscZawodnika();
            value.WartoscRynkowa = random.Next(oldValue/2, oldValue*3/2);
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

        private TTransferZawodnika generateTransfer(Zawodnik player, DateTime date, int newClubId, int price)
        {
            TTransferZawodnika transfer = new TTransferZawodnika();
            transfer.DataTransferu = date;
            transfer.KlubKupujacyId = newClubId;
            transfer.KlubSprzedajacyId = player.KlubId;
            transfer.KwotaTransferu = price;
            transfer.TypPlatnosci = getRandomPaymentType();
            transfer.ZawodnikId = player.Id;

            return transfer;
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
            List<TWartoscZawodnika> playersValues = new List<TWartoscZawodnika>();
            int minPlayerId = getPlayerMinId(), maxPlayerId = getPlayerMaxId();

            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var prices = dbContext.WartoscZawodnika.AsNoTracking().ToList();

                for (int i = minPlayerId; i <= maxPlayerId; i++)
                {
                    TWartoscZawodnika value;
                    if (prices.Count() > 0)
                    {
                        var playerValuesBeforeDate = prices.Where(x => x.ZawodnikId == i && x.DataWystawienia <= time).ToList();
                        var player = playerValuesBeforeDate.OrderByDescending(x => x.DataWystawienia).First();
                        prices.Remove(player);
                        value = generateValueBasedOnLastOne(i, time, player.WartoscRynkowa.GetValueOrDefault());
                    }
                    else
                    {
                        value = generateValue(i, time);
                    }
                    
                    playersValues.Add(value);
                    table.Rows.Add(value.Id, value.WartoscRynkowa, value.DataWystawienia, value.ZawodnikId);
                }
            }
            

            using (var bulk = new SqlBulkCopy("Server= DESKTOP-J5I9Q9P; Initial Catalog = DataWarehousesProject; integrated security=true"))
            {
                bulk.DestinationTableName = "WartoscZawodnika";
                bulk.WriteToServer(table);
            }
        }

        public void generateNTransfers(int n, DateTime date)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("DataWystawienia", typeof(DateTime));
            table.Columns.Add("TypPlatnosci", typeof(string));
            table.Columns.Add("KlubSprzedajacyId", typeof(Int32));
            table.Columns.Add("KlubKupujacyId", typeof(Int32));
            table.Columns.Add("KwotaTransferu", typeof(Int32));
            table.Columns.Add("ZawodnikId", typeof(Int32));

            int maxClubId = getClubMaxId(), minClubId = getClubMinId();
            int maxPlayerId = getPlayerMaxId(), minPlayerId = getPlayerMinId();

            List<Zawodnik> players = new List<Zawodnik>();
            List<Zawodnik> updatedPlayers = new List<Zawodnik>();
            List<WartoscZawodnika> prices = new List<WartoscZawodnika>();

            using(var dbContext = new DataWarehousesProjectEntities())
            {
                players = dbContext.Zawodnik.AsNoTracking().ToList();
                prices = dbContext.WartoscZawodnika.AsNoTracking().ToList();

                int playerId;
                List<int> playerTransferred = new List<int>();

                for(int i = 0; i<n; i++) //n cannot be greater than number of players
                {
                    do
                    {
                        playerId = random.Next(minPlayerId, maxPlayerId);
                    } while (playerTransferred.Contains(playerId)); //give us random player to transfer
                    playerTransferred.Add(playerId);

                    Zawodnik player = players.Where(x => x.Id == playerId).FirstOrDefault();

                    int newClubId = random.Next(minClubId, maxClubId), price;
                    if (newClubId == player.KlubId) newClubId++;
                    int playerAge = date.Year - player.DataUrodzenia.Year;

                    //was faster but can cause stupid results
                    //int playerValue = prices.Where(x => x.ZawodnikId == player.Id).FirstOrDefault().WartoscRynkowa.GetValueOrDefault();
                    
                    //works too slow 
                    var playerValuesBeforeDate = prices.Where(x => x.ZawodnikId == player.Id && x.DataWystawienia <= date).ToList();
                    int playerValue = playerValuesBeforeDate.OrderByDescending(x => x.DataWystawienia).First().WartoscRynkowa.GetValueOrDefault();         

                    if (playerAge < 23)
                    {
                        price = random.Next(playerValue,2*playerValue);
                    }
                    else if(playerAge > 32)
                    {
                        price = random.Next(playerValue/3, playerValue);
                    }
                    else
                    {
                        price = random.Next(playerValue/2, 3*playerValue/2);
                    }


                    var transfer = generateTransfer(player, date, newClubId, price);
                    int days = random.Next(0, 30);
                    DateTime transferDate = transfer.DataTransferu.AddDays(days);
                    if (days > 20 && days < 29) transfer.KwotaTransferu += transfer.KwotaTransferu / 20; //+5% of value
                    else if (days > 28) transfer.KwotaTransferu += transfer.KwotaTransferu / 10; //+10% of value


                    table.Rows.Add(transfer.Id, transferDate, transfer.TypPlatnosci, transfer.KlubSprzedajacyId, transfer.KlubKupujacyId, transfer.KwotaTransferu, transfer.ZawodnikId);
                    player.KlubId = transfer.KlubKupujacyId;                  
                    updatedPlayers.Add(player);
                }


                foreach(var player in updatedPlayers)
                {
                    var playerToUpadate = dbContext.Zawodnik.AsNoTracking().Where(x => x.Id == player.Id).FirstOrDefault(); 
                    if (player.KlubId != playerToUpadate.KlubId)
                    {
                        playerToUpadate.KlubId = player.KlubId;
                    }
                    dbContext.SaveChanges();
                }
               
            }

            using (var bulk = new SqlBulkCopy("Server= DESKTOP-J5I9Q9P; Initial Catalog = DataWarehousesProject; integrated security=true"))
            {
                bulk.DestinationTableName = "TransferZawodnika";
                bulk.WriteToServer(table);
            }
        }


        private int getPlayerMaxId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var playerId = dbContext.Zawodnik.AsNoTracking().Max(x => x.Id);
                return playerId;
            }
        }

        private int getPlayerMinId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var playerId = dbContext.Zawodnik.AsNoTracking().Min(x => x.Id);
                return playerId;
            }
        }

        private int getClubMaxId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var clubId = dbContext.Klub.AsNoTracking().Max(x => x.Id);
                return clubId;
            }
        }

        private int getClubMinId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var clubId = dbContext.Klub.AsNoTracking().Min(x => x.Id);
                return clubId;
            }
        }

        private int getAgentMaxId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var agentId = dbContext.Agent.AsNoTracking().Max(x => x.Id);
                return agentId;
            }
        }

        public int getAgentMinId()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                var agentId = dbContext.Agent.AsNoTracking().Min(x => x.Id);
                return agentId;
            }
        }
    }
}
