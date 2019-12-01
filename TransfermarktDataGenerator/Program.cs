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
            CreateDataBetweenTwoTimeMoments(new DateTime(2017, 6, 30), new DateTime(2018, 1, 1), 5000); //now generating milion of records toogether in between T1 and T2 + 7500 transfers (the longest operation)
            //DataGenerator dataGenerator = new DataGenerator();
            //dataGenerator.generateNTransfers(1, new DateTime(2020, 6, 30));
        }

        static public void CreateDataBetweenTwoTimeMoments(DateTime T1, DateTime T2, int amount)
        {
            DataGenerator dataGenerator = new DataGenerator();
            dataGenerator.generateNClubs(amount / 25);
            checkIfAllClubsAreUnique();
            dataGenerator.generateNAgents(amount);
            dataGenerator.generateNPlayers(amount);
            dataGenerator.generatePlayersValues(T1);
            dataGenerator.generateNTransfers(1000, T1);


            //T2 moment data
            dataGenerator.generateNAgents(amount);
            dataGenerator.generateNPlayers(amount);
            dataGenerator.generatePlayersValues(T2);
            dataGenerator.generateNTransfers(1000, T2);

            //in the end get all players iterate through all of them and: player.pesel += player.Id.ToString();
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                List<Zawodnik> listOfPlayers = dbContext.Zawodnik.ToList();
                foreach (Zawodnik zawodnik in listOfPlayers)
                {
                    zawodnik.pesel += zawodnik.Id.ToString();
                }
                dbContext.SaveChanges();
            }
            //same with agents
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                List<Agent> listOfAgents = dbContext.Agent.ToList();
                foreach (Agent agent in listOfAgents)
                {
                    agent.pesel += agent.Id.ToString();
                }
                dbContext.SaveChanges();
            }
            //poprzedni transfer za mniejszą większą kwotę i dlugosc pobytu w klubie
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                List<TransferZawodnika> allTransfers = dbContext.TransferZawodnika.ToList();
                foreach (TransferZawodnika transfer in allTransfers)
                {
                    List<TransferZawodnika> transferCheck = allTransfers.Where(x => x.ZawodnikId == transfer.ZawodnikId).ToList();
                    string PoprzedniTransferZaMniejszaWiekszaKwote;
                    string dlugoscPobytuWKlubie;
                    if (transferCheck.Count() == 1)
                    {
                        PoprzedniTransferZaMniejszaWiekszaKwote = "BrakTransferow";
                        dlugoscPobytuWKlubie = "PonadDwaLata";
                    }
                    else
                    {
                        TransferZawodnika toCompare = transferCheck.Where(x => x.Id != transfer.Id).FirstOrDefault();
                        if (DateTime.Compare(toCompare.DataTransferu, transfer.DataTransferu) < 0) //there was ealier transfer
                        {
                            if (toCompare.KwotaTransferu - transfer.KwotaTransferu > 0)
                                PoprzedniTransferZaMniejszaWiekszaKwote = "Wieksza";
                            else if (toCompare.KwotaTransferu - transfer.KwotaTransferu < 0)
                                PoprzedniTransferZaMniejszaWiekszaKwote = "Mniejsza";
                            else
                                PoprzedniTransferZaMniejszaWiekszaKwote = "Rowna";

                            int differenceInMonths = ((transfer.DataTransferu.Year - toCompare.DataTransferu.Year) * 12) + transfer.DataTransferu.Month - toCompare.DataTransferu.Month;
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

                    transfer.PoprzedniTransferZaWiekszaMniejszaKwote = PoprzedniTransferZaMniejszaWiekszaKwote;
                    transfer.DlugoscPobytuWKlubie = dlugoscPobytuWKlubie;
                }
                dbContext.SaveChanges();
            }

            using (var dbContext = new DataWarehousesProjectEntities())
            {
                List<WartoscZawodnika> allWartosc = dbContext.WartoscZawodnika.ToList();
                foreach(WartoscZawodnika wartosc in allWartosc)
                {
                    List<WartoscZawodnika> singlePlayerValues = allWartosc.Where(x => x.ZawodnikId == wartosc.ZawodnikId).ToList(); //return one player values sorted by date
                    float LastValue = 0, wspolczynnikZmianyWartosci = 1;
                    foreach(WartoscZawodnika wz in singlePlayerValues)
                    {
                        if (LastValue != 0)
                        {
                            wspolczynnikZmianyWartosci = (float)wz.WartoscRynkowa.GetValueOrDefault() / LastValue;
                        }
                        LastValue = (int)wz.WartoscRynkowa.GetValueOrDefault();
                        wz.WspolczynnikZmianyWartosci = wspolczynnikZmianyWartosci;
                    }
                }


                dbContext.SaveChanges();
            }
        }

        static private void checkIfAllClubsAreUnique()
        {
            using (var dbContext = new DataWarehousesProjectEntities())
            {
                List<Klub> clubs = dbContext.Klub.ToList();
                foreach (Klub club in clubs)
                {
                    if (clubs.Where(x => x.Nazwa.ToUpper() == club.Nazwa.ToUpper()).Count() > 1)
                    {
                        dbContext.Klub.Remove(club);
                    }
                }
                dbContext.SaveChanges();
            }
        }
    }
}
