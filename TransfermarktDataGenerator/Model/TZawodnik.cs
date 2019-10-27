using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarktDataGenerator.Model
{
    class TZawodnik
    {
        public int Id { get; set; }
        public string Imię { get; set; }
        public string Nazwisko { get; set; }
        public System.DateTime DataUrodzenia { get; set; }
        public string Pozycja { get; set; }
        public Nullable<int> KlubId { get; set; }
        public Nullable<int> AgentId { get; set; }
    }
}
