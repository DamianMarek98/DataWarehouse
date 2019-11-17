using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Smieci
    {
        private int Id;
        private string TypPlatnosci;
        private string DlugoscPobytuWKlubie;
        private string PoprzedniTransferZaMniejszaWiekszaKwote;

        public Smieci(int id, string typPlatnosci, string dlugoscPobytuWKlubie, string poprzedniTransferZaMniejszaWiekszaKwote)
        {
            Id = id;
            TypPlatnosci = typPlatnosci;
            DlugoscPobytuWKlubie = dlugoscPobytuWKlubie;
            PoprzedniTransferZaMniejszaWiekszaKwote = poprzedniTransferZaMniejszaWiekszaKwote;
        }

        override public string ToString()
        {
            string Smieci = ("(" + Id.ToString() + ", " + "'" + TypPlatnosci + "'" + ", " + "'" + DlugoscPobytuWKlubie + "'" + "," + "'" + PoprzedniTransferZaMniejszaWiekszaKwote + "'" + ")");
            return Smieci;
        }
    }
}
