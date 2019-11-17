using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator.Model
{
    class Agent
    {
        public int Id;
        private string ImieINazwisko;

        public Agent(int id, string imieINazwisko)
        {
            Id = id;
            ImieINazwisko = imieINazwisko;
        }

        override public string ToString()
        {
            string agent = ("(" + Id.ToString() + ", " + "'" +ImieINazwisko + "'" + ")");
            return agent;
        }

    }
}
