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
    
    public partial class TipoQuestao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoQuestao()
        {
            this.Questao = new HashSet<Questao>();
        }
    
        public int CodTipoQuestao { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }
    }
}
