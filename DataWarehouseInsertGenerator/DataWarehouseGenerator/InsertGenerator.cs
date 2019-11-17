using DataWarehouseGenerator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVGenerator;
using CSVGenerator.Model;

namespace DataWarehouseGenerator
{
    class InsertGenerator
    {
        private int transfersAmount;
        private string filePath = @"C:\Users\Deny\Desktop\Studia\V Sem\DataWarehouses\DataWarehouseInsertGenerator\DataWarehouseGenerator\inserts.txt";
        //lists to store data form generated database
        private List<Agent> agents = new List<Agent>();
        private List<Klub> clubs = new List<Klub>();
        private List<TransferZawodnika> playerTransfers = new List<TransferZawodnika>();
        private List<WartoscZawodnika> playerValues = new List<WartoscZawodnika>();
        private List<Zawodnik> players = new List<Zawodnik>();
        private List<PlayerContractFullInfo> papers = new List<PlayerContractFullInfo>();

        //lists to store data to create inserts
        private List<Model.Dzien> dzien = new List<Dzien>();
        private List<Model.Agent> agent = new List<Model.Agent>();
        private List<Model.Czas> czas = new List<Czas>();
        private List<Model.Klub> klub = new List<Model.Klub>();
        private List<Model.Kontrakt> kontrakt = new List<Kontrakt>();
        private List<Model.Smieci> smieci = new List<Smieci>();
        private List<Model.TransferZawodnika> transferZawodnika = new List<Model.TransferZawodnika>();
        private List<Model.WartoscZawodnika> wartoscZawodnika = new List<Model.WartoscZawodnika>();
        private List<Model.Zawodnik> zawodnik = new List<Model.Zawodnik>();

        public InsertGenerator(int amount)
        {
            transfersAmount = amount;
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                agents = dbContext.Agent.AsNoTracking().ToList();
                clubs = dbContext.Klub.AsNoTracking().ToList();
                playerTransfers = dbContext.TransferZawodnika.AsNoTracking().ToList();
                playerValues = dbContext.WartoscZawodnika.AsNoTracking().ToList();
                players = dbContext.Zawodnik.AsNoTracking().ToList();
            }
            papers = CSVReader.ReadData();
        }

        private void selectData()
        {
            var random = new Random();
            for (int i=0; i<transfersAmount; i++)
            {
                int id = random.Next(playerTransfers.Count());
                while (!(transferZawodnika.Where(x => x.Id == id).Count() == 0))
                {
                    id = random.Next(playerTransfers.Count());
                }

                 var playerTransfer = playerTransfers[id];

                //basing on transfer get 1. Get klub kupujący and sprzedający 2. get zawodnik 3. get Agent basing on player 4. get czas on Transfer Zawodnika 5.get all wartoscZawodnika for each zawodnik 6. for each wartosc get date 7. smiec 8. transferZawodnika 9.kontrakt

                //Kluby
                int buyingClubId = playerTransfer.KlubKupujacyId.GetValueOrDefault(), sellingClubId = playerTransfer.KlubSprzedajacyId.GetValueOrDefault();
                var buyingClub = clubs.Where(x => x.Id == buyingClubId).FirstOrDefault();
                var sellingClub = clubs.Where(x => x.Id == sellingClubId).FirstOrDefault();
                Model.Klub klubKupujacy = new Model.Klub(buyingClub.Id, buyingClub.Nazwa, buyingClub.Kraj, buyingClub.Budżet.GetValueOrDefault());
                Model.Klub klubSprzedajacy = new Model.Klub(sellingClub.Id, sellingClub.Nazwa, sellingClub.Kraj, sellingClub.Budżet.GetValueOrDefault());

                int klubKupujacyDuplicate = klub.Where(x => x.Id == buyingClub.Id).Count();
                int klubSprzedajacyDuplicate = klub.Where(x => x.Id == sellingClub.Id).Count();
                bool klubKupujacyFlag = true, klubSprzedajacyFlag = true;
                if (klubKupujacyDuplicate != 0) klubKupujacyFlag = false;
                if (klubSprzedajacyDuplicate != 0) klubSprzedajacyFlag = false;

                if (klubKupujacyFlag)
                    klub.Add(klubKupujacy);
                if (klubSprzedajacyFlag)
                    klub.Add(klubSprzedajacy);

                //Zawodnik
                var player = players.Where(x => x.Id == playerTransfer.ZawodnikId).FirstOrDefault();
                //zawondik status i przedzial wiekowy
                int age = DateTime.Today.Year - player.DataUrodzenia.Year;
                string przedzialWiekowy;
                if (age < 24)
                    przedzialWiekowy = "od 15 do 23";
                else if (age < 31)
                    przedzialWiekowy = "od 24 do 30";
                else
                    przedzialWiekowy = "od 31 do 45";

                string statusWKlubie;
                if (playerTransfers.Where(x => x.ZawodnikId == player.Id).Count() == 0)
                    statusWKlubie = "Wychowanek";
                else
                    statusWKlubie = "ZPozaKlubu";

                Model.Zawodnik zawodnikToAdd = new Model.Zawodnik(player.Id, player.Pozycja, player.KlubId.GetValueOrDefault() , player.AgentId.GetValueOrDefault(), przedzialWiekowy, player.Imię+" "+player.Nazwisko, statusWKlubie);
                if(zawodnik.Where(x => x.Id == player.Id).Count() == 0)
                    zawodnik.Add(zawodnikToAdd);

                //agent
                var agentToAdd = agents.Where(x => x.Id == player.AgentId).FirstOrDefault();
                Model.Agent agentCreated = new Model.Agent(agentToAdd.Id, agentToAdd.Imię+" "+agentToAdd.Nazwisko);
                if (agent.Where(x => x.Id == agentToAdd.Id).Count() == 0)
                    agent.Add(agentCreated);

                //czas
                int year = playerTransfer.DataTransferu.Year, month = playerTransfer.DataTransferu.Month, day = playerTransfer.DataTransferu.Day;
                string DzienOkienka, PrzynaleznoscDaty;
                if(month >=6 && month <=8)  //letnie okienko
                {
                    PrzynaleznoscDaty = "LetnieOkienko";
                    if (month == 8 && day == 31)
                        DzienOkienka = "OstatniDzienOkienka";
                    if (month == 8 && day >= 24 && day < 31)
                        DzienOkienka = "OstatniTydzienOkienka";
                    else
                        DzienOkienka = "WTrakcieOkienka";
                }
                else if(month == 1) //zimowe
                {
                    PrzynaleznoscDaty = "ZimoweOkienko";
                    if (day == 31)
                        DzienOkienka = "OstatniDzienOkienka";
                    if (day >= 24 && day < 31)
                        DzienOkienka = "OstatniTydzienOkienka";
                    else
                        DzienOkienka = "WTrakcieOkienka";
                }
                else
                {
                    PrzynaleznoscDaty = "PozaOkienkiem";
                    DzienOkienka = "NieWTrakcieOkienka";
                }

                Model.Czas czasToAdd = new Model.Czas(czas.Count()+1, year, month, day, DzienOkienka, PrzynaleznoscDaty);
                czas.Add(czasToAdd); //id == czas.Count();

                //wartosci zawodnika
                List<WartoscZawodnika> singlePlayerValues = playerValues.Where(x => x.ZawodnikId == playerTransfer.ZawodnikId).ToList(); //return one player values sorted by date
                double LastValue = 0, wspolczynnikZmianyWartosci = 1;
              
                foreach(WartoscZawodnika wz in singlePlayerValues)
                {
                    if (LastValue != 0)
                    {
                        wspolczynnikZmianyWartosci = (double) wz.WartoscRynkowa.GetValueOrDefault() / LastValue;
                    }

                    //add dzien
                    year = wz.DataWystawienia.Year;
                    day = wz.DataWystawienia.Day;
                    Model.Dzien dzienToAdd = new Model.Dzien(dzien.Count()+1, wz.DataWystawienia, year, getMonthNameByNumber(wz.DataWystawienia.Month), day);
                    dzien.Add(dzienToAdd); //id = czas.Count;
                    LastValue = wz.WartoscRynkowa.GetValueOrDefault();

                    //add player value
                    if ((wartoscZawodnika.Where(x => x.Id == wz.Id).Count() == 0))
                    {
                        Model.WartoscZawodnika wartoscZawodnikaToAdd = new Model.WartoscZawodnika(wz.Id, wz.WartoscRynkowa.GetValueOrDefault(), czas.Count(), wz.ZawodnikId.GetValueOrDefault(), wspolczynnikZmianyWartosci);
                        wartoscZawodnika.Add(wartoscZawodnikaToAdd);
                    }
                }

                //smiec
                List<TransferZawodnika> transfersCheck = playerTransfers.Where(x => x.ZawodnikId == playerTransfer.ZawodnikId).ToList(); //in data generator we have max 2 transfers
                string PoprzedniTransferZaMniejszaWiekszaKwote;
                string dlugoscPobytuWKlubie;
                if (transfersCheck.Count() == 1)
                {
                    PoprzedniTransferZaMniejszaWiekszaKwote = "BrakTransferow";
                    dlugoscPobytuWKlubie = "PonadDwaLata";
                }
                else
                {
                    TransferZawodnika toCompare = transfersCheck.Where(x => x.Id != playerTransfer.Id).FirstOrDefault();
                    Console.WriteLine(toCompare.DataTransferu.ToString() + ", a data transferu = " + playerTransfer.DataTransferu.ToString());
                    if (DateTime.Compare(toCompare.DataTransferu, playerTransfer.DataTransferu) <0 ) //there was ealier transfer
                    {
                        if (toCompare.KwotaTransferu - playerTransfer.KwotaTransferu > 0)
                            PoprzedniTransferZaMniejszaWiekszaKwote = "Wieksza";
                        else if (toCompare.KwotaTransferu - playerTransfer.KwotaTransferu < 0)
                            PoprzedniTransferZaMniejszaWiekszaKwote = "Mniejsza";
                        else
                            PoprzedniTransferZaMniejszaWiekszaKwote = "Rowna";
                       
                        int differenceInMonths = ((playerTransfer.DataTransferu.Year - toCompare.DataTransferu.Year) * 12) + playerTransfer.DataTransferu.Month - toCompare.DataTransferu.Month;
                        if (differenceInMonths >= 24)
                            dlugoscPobytuWKlubie = "PonadDwaLata";
                        else if (differenceInMonths >= 12)
                            dlugoscPobytuWKlubie = "DoDwochLat";
                        else
                            dlugoscPobytuWKlubie = "DoRoku";
                    }
                    else
                    {
                        PoprzedniTransferZaMniejszaWiekszaKwote = "BrakTransferow";
                        dlugoscPobytuWKlubie = "PonadDwaLata";
                    }
                }

                Model.Smieci smiecToAdd = new Model.Smieci(smieci.Count() + 1, playerTransfer.TypPlatnosci, dlugoscPobytuWKlubie, PoprzedniTransferZaMniejszaWiekszaKwote);
                smieci.Add(smiecToAdd); //id = smieci.count()

                Model.TransferZawodnika transferToAdd = new Model.TransferZawodnika(playerTransfer.Id, czas.Count(), smieci.Count(), playerTransfer.KlubSprzedajacyId.GetValueOrDefault(), playerTransfer.KlubKupujacyId.GetValueOrDefault(), playerTransfer.KwotaTransferu.GetValueOrDefault(), playerTransfer.ZawodnikId);
                transferZawodnika.Add(transferToAdd);
            }

            //kontakty
            foreach (Model.Zawodnik z in zawodnik) 
            {
                List<PlayerContractFullInfo> kontrkatyGracza = papers.Where(x => x.playerId == z.Id).ToList();
                foreach (PlayerContractFullInfo k in kontrkatyGracza)
                {
                    //iloscmiesiecy, pensja miesieczna
                    //add data startu
                    Model.Dzien dzienToAdd = new Model.Dzien(dzien.Count() + 1, k.signingDay, k.signingDay.Year, getMonthNameByNumber(k.signingDay.Month), k.signingDay.Day);
                    dzien.Add(dzienToAdd); //id = czas.Count;
                    //add data zakonczenia
                    Model.Dzien dzienZToAdd = new Model.Dzien(dzien.Count() + 1, k.expiryDay, k.expiryDay.Year, getMonthNameByNumber(k.expiryDay.Month), k.expiryDay.Day);
                    dzien.Add(dzienZToAdd); //id = czas.Count;

                    int lengthInMonths = ((k.expiryDay.Year - k.signingDay.Year) * 12) + k.expiryDay.Month - k.signingDay.Month;
                    int salaryPerMonth = (int) k.salary / lengthInMonths;

                    Model.Kontrakt kontraktToAdd = new Kontrakt(kontrakt.Count()+1, z.AgentId, z.Id, (int)k.commission, salaryPerMonth, dzienToAdd.Id, dzienZToAdd.Id,(int) (k.commission+k.variablesAmount+k.salary), lengthInMonths, (int)k.variablesAmount, k.wereVariablesPaid );
                    kontrakt.Add(kontraktToAdd);
                }

            }
        }

        private string getMonthNameByNumber(int n)
        {
            switch (n)
            {
                case 1: return "styczen";
                case 2: return "luty";
                case 3: return "marzec";
                case 4: return "kwiecien";
                case 5: return "maj";
                case 6: return "czerwiec";
                case 7: return "lipiec";
                case 8: return "sierpien";
                case 9: return "wrzesien";
                case 10: return "pazdziernik";
                case 11: return "listopad";
                case 12: return "grudzien";
                default: return "notPossible";
            }
        }

        public void Generate()
        {
            selectData();

            if (File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    //klub
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. Klub ON");
                    sw.WriteLine("insert into Klub (Id, Nazwa, Kraj, Budżet) Values");
                    //now values
                    var lastKlub = klub.Distinct().Last();

                    foreach (Model.Klub klub in klub.Distinct())
                    {
                        if (!klub.Equals(lastKlub))
                        {
                            sw.WriteLine(klub.ToString()+",");
                        }
                        else
                        {
                            sw.WriteLine(klub.ToString()+";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. Klub OFF");
                    //agent
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. Agent ON");
                    sw.WriteLine("insert into Agent(Id, ImieINazwisko) Values");
                    //now values
                    var lastAgent = agent.Last();
                    foreach (Model.Agent agent in agent)
                    {
                        if (!agent.Equals(lastAgent))
                        {
                            sw.WriteLine(agent.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(agent.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. Agent OFF");
                    //czas
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. czas ON");
                    sw.WriteLine("insert into czas(Id, Rok, Miesiac, Dzien, DzienOkienka, PrzynaleznoscDaty) Values");
                    //now values
                    var lastCzas = czas.Last();
                    foreach (Model.Czas c in czas)
                    {
                        if (!c.Equals(lastCzas))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. czas OFF");
                    //dzien
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. Dzien ON");
                    sw.WriteLine("insert into Dzien(Id, DokladnaData, Rok, Miesiac, Dzien) Values");
                    //now values
                    var lastD = dzien.Last();
                    foreach (Model.Dzien c in dzien)
                    {
                        if (!c.Equals(lastD))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. Dzien OFF");
                    //smieci
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. Smieci ON");
                    sw.WriteLine("insert into Smieci(Id, TypPlatnosci, DlugoscPobytuWKlubie, PoprzedniTransferZaMniejszaWiekszaKwote) Values");
                    //now values
                    var lastS = smieci.Last();
                    foreach (Model.Smieci c in smieci)
                    {
                        if (!c.Equals(lastS))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. Smieci OFF");
                    //zawodnik
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. Zawodnik ON");
                    sw.WriteLine("insert into Zawodnik(Id, Pozycja, KlubId, AgentId, PrzedzialWiekowy, ImieINazwisko, StatusWKlubie) Values");
                    //now values
                    var lastZ = zawodnik.Last();
                    foreach (Model.Zawodnik c in zawodnik)
                    {
                        if (!c.Equals(lastZ))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. Zawodnik OFF");
                    //wartoscZawodniak
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. WartoscZawodnika ON");
                    sw.WriteLine("insert into WartoscZawodnika(Id, WartoscRynkowa, DataWystawienia, ZawodnikId, wspolczynnikZmianyWartosci) Values");
                    //now values
                    var lastW = wartoscZawodnika.Last();
                    foreach (Model.WartoscZawodnika c in wartoscZawodnika)
                    {
                        if (!c.Equals(lastW))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. WartoscZawodnika OFF");
                    //transferZawodnika
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. TransferZawodnika ON");
                    sw.WriteLine("insert into TransferZawodnika(Id, DataTransferuId, SmieciId, KlubSprzedajacyId, KlubKupujacyId, KwotaTransferu, ZawodnikId) Values");
                    //now values
                    var lastT = transferZawodnika.Last();
                    foreach (Model.TransferZawodnika c in transferZawodnika)
                    {
                        if (!c.Equals(lastT))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. TransferZawodnika OFF");
                    //kontrakt
                    sw.WriteLine("SET IDENTITY_INSERT TRANSFERMARKT. dbo. Kontrakt ON");
                    sw.WriteLine("insert into Kontrakt(Id, AgentId, ZawodnikId, prowizjaAgenta, pesjaMiesieczna, DataStartu, DataZakonczenia, WartoscKontraktu, iloscMiesiecy, Zmienna, CzyZmiennaWyplacona) Values");
                    //now values
                    var lastK = kontrakt.Last();
                    foreach (Model.Kontrakt c in kontrakt)
                    {
                        if (!c.Equals(lastK))
                        {
                            sw.WriteLine(c.ToString() + ",");
                        }
                        else
                        {
                            sw.WriteLine(c.ToString() + ";");
                        }
                    }
                    sw.WriteLine("SET IDENTITY_INSERT Transfermarkt. dbo. Kontrakt OFF");
                }
            }
        }


    }
}
