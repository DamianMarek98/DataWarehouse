using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarktDataGenerator.Model
{
    class TKlub
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Kraj { get; set; }
        public Nullable<decimal> Budżet { get; set; }
    }
}
