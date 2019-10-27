﻿using System;
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
    }
}