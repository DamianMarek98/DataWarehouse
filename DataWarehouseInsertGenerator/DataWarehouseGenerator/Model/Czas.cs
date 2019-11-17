using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Czas
    {
        private int Id;
        private int Rok;
        private int Miesiac;
        private int Dzien;
        private string DzienOkienka;
        private string PrzynaleznoscDaty;

        public Czas(int id, int rok, int miesiac, int dzien, string dzienOkienka, string przynaleznoscDaty)
        {
            Id = id;
            Rok = rok;
            Miesiac = miesiac;
            Dzien = dzien;
            DzienOkienka = dzienOkienka;
            PrzynaleznoscDaty = przynaleznoscDaty;
        }

        override public string ToString()
        {
            string czas = ("(" + Id.ToString() + ", " + Rok.ToString() + ", " + Miesiac.ToString() + ", " + Dzien.ToString()+"," + "'" + DzienOkienka + "'" + "," + "'" + PrzynaleznoscDaty + "'" + ")");
            return czas;
        }
    }
}
