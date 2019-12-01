using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarktDataGenerator.Model
{
    class TWartoscZawodnika
    {
        public int Id { get; set; }
        public Nullable<decimal> WartoscRynkowa { get; set; }
        public System.DateTime DataWystawienia { get; set; }
        public Nullable<int> ZawodnikId { get; set; }
        public float WspolczynnikZmianyWartosci { get; set; }
    }
}
