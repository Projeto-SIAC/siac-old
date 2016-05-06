//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIAC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SimCandidatoProva
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SimCandidatoProva()
        {
            this.SimCandidatoQuestao = new HashSet<SimCandidatoQuestao>();
        }
    
        public int Ano { get; set; }
        public int NumIdentificador { get; set; }
        public int CodCandidato { get; set; }
        public int CodDiaRealizacao { get; set; }
        public int CodProva { get; set; }
        public Nullable<decimal> NotaDiscursiva { get; set; }
        public Nullable<int> QteAcertos { get; set; }
        public Nullable<bool> FlagPresente { get; set; }
        public Nullable<decimal> EscorePadronizado { get; set; }
        public string Observacoes { get; set; }
    
        public virtual SimCandidato SimCandidato { get; set; }
        public virtual SimProva SimProva { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimCandidatoQuestao> SimCandidatoQuestao { get; set; }
    }
}