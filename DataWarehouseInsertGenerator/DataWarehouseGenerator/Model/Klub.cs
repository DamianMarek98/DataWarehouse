using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Klub
    {
        public int Id;
        public string Nazwa;
        public string Kraj;
        public int Budżet;

        public Klub(int id, string nazwa, string kraj, int budżet)
        {
            Id = id;
            Nazwa = nazwa;
            Kraj = kraj;
            Budżet = budżet;
        }

        override public string ToString()
        {
            string klub = ("("+Id.ToString() + ", " + "'" + Nazwa + "'" + ", " + "'" + Kraj + "'" + ", " + Budżet.ToString()+")");
            return klub;
        }
    }
}
