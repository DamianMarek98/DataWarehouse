using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Zawodnik
    {
        public int Id;
        private string Pozycja;
        public int KlubId;
        public int AgentId;
        private string PrzedzialWiekowy;
        private string ImieINazwisko;
        private string StatusWKlubie;

        public Zawodnik(int id, string pozycja, int klubId, int agentId, string przedzialWiekowy, string imieINazwisko, string statusWKlubie)
        {
            Id = id;
            Pozycja = pozycja;
            KlubId = klubId;
            AgentId = agentId;
            PrzedzialWiekowy = przedzialWiekowy;
            ImieINazwisko = imieINazwisko;
            StatusWKlubie = statusWKlubie;
        }

        override public string ToString()
        {
            string z = ("(" + Id.ToString() + ", " + "'" + Pozycja + "'" + ", " + KlubId.ToString() + ", " + AgentId.ToString() + "," + "'" + PrzedzialWiekowy + "'" + "," + "'" + ImieINazwisko + "'" + "," + "'" + StatusWKlubie + "'" + ")");
            return z;
        }
    }
}
