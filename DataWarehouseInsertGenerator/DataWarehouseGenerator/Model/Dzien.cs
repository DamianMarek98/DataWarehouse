using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Dzien
    {
        public int Id;
        private DateTime DokladnaData;
        private int Rok;
        private string Miesiac;
        private int day;

        public Dzien(int id, DateTime dokladnaData, int rok, string miesiac, int dzien)
        {
            Id = id;
            DokladnaData = dokladnaData;
            Rok = rok;
            Miesiac = miesiac;
            day = dzien;
        }

        override public string ToString()
        {
            string dzien = ("(" + Id.ToString() + ", " + "'" + DokladnaData.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + ", " + Rok.ToString() + ", " + "'" + Miesiac + "'" + "," + day.ToString() + ")");
            return dzien;
        }
    }
}
