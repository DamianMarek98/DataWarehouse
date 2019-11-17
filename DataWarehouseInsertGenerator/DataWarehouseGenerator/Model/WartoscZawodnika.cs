using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class WartoscZawodnika
    {
        public int Id;
        private int WartoscRynkowa;
        private int DataWystawienia;
        private int ZawodnikId;
        private double wspolczynnikZmianyWartosci;

        public WartoscZawodnika(int id, int wartoscRynkowa, int dataWystawienia, int zawodnikId, double wspolczynnikZmianyWartosci)
        {
            Id = id;
            WartoscRynkowa = wartoscRynkowa;
            DataWystawienia = dataWystawienia;
            ZawodnikId = zawodnikId;
            this.wspolczynnikZmianyWartosci = wspolczynnikZmianyWartosci;
        }

        override public string ToString()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            string w = ("(" + Id.ToString() + ", " + WartoscRynkowa.ToString() + ", " + DataWystawienia.ToString() + ", " + ZawodnikId.ToString() + "," + wspolczynnikZmianyWartosci.ToString(nfi) + ")");
            return w;
        }
    }
}
