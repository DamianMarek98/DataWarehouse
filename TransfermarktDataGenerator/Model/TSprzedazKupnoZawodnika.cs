using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarktDataGenerator.Model
{
    class TSprzedazKupnoZawodnika
    {
        public int Id { get; set; }
        public Nullable<int> KwotaTransferu { get; set; }
        public Nullable<int> ZawodnikId { get; set; }
        public Nullable<int> TransferId { get; set; }
    }
}
