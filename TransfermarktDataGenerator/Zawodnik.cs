//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransfermarktDataGenerator
{
    using System;
    using System.Collections.Generic;
    
    public partial class Zawodnik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Zawodnik()
        {
            this.TransferZawodnika = new HashSet<TransferZawodnika>();
            this.WartoscZawodnika = new HashSet<WartoscZawodnika>();
        }
    
        public int Id { get; set; }
        public string Imię { get; set; }
        public string Nazwisko { get; set; }
        public System.DateTime DataUrodzenia { get; set; }
        public string Pozycja { get; set; }
        public Nullable<int> KlubId { get; set; }
        public Nullable<int> AgentId { get; set; }
        public string pesel { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual Klub Klub { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransferZawodnika> TransferZawodnika { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WartoscZawodnika> WartoscZawodnika { get; set; }
    }
}
