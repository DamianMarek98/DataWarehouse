using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class TransferZawodnika
    {
        public int Id;
        private int DataTransferuId;
        private int SmieciId;
        private int KlubSprzedajacyId;
        private int KlubKupujacyId;
        private int KwotaTransferu;
        private int ZawodnikId;

        public TransferZawodnika(int id, int dataTransferuId, int smieciId, int klubSprzedajacyId, int klubKupujacyId, int kwotaTransferu, int zawodnikId)
        {
            Id = id;
            DataTransferuId = dataTransferuId;
            SmieciId = smieciId;
            KlubSprzedajacyId = klubSprzedajacyId;
            KlubKupujacyId = klubKupujacyId;
            KwotaTransferu = kwotaTransferu;
            ZawodnikId = zawodnikId;
        }

        override public string ToString()
        {
            string t = ("(" + Id.ToString() + ", " + DataTransferuId.ToString() + ", " + SmieciId.ToString() + ", " + KlubSprzedajacyId.ToString() + "," + KlubKupujacyId.ToString() + "," + KwotaTransferu.ToString() + "," + ZawodnikId.ToString() + ")");
            return t;
        }
    }
}
