using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarktDataGenerator.Model
{
    class TTransferZawodnika
    {
        public int Id { get; set; }
        public System.DateTime DataTransferu { get; set; }
        public string TypPlatnosci { get; set; }
        public Nullable<int> KlubSprzedajacyId { get; set; }
        public Nullable<int> KlubKupujacyId { get; set; }
        public Nullable<decimal> KwotaTransferu { get; set; }
        public Nullable<int> ZawodnikId { get; set; }
        public string TransferZaMniejszaWiekszaKwote { get; set; }
        public string DlugoscPobytuWKlubie { get; set; }
    }
}
