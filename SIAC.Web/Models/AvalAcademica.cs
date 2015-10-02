//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIAC.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AvalAcademica
    {
        public int NumIdentificador { get; set; }
        public int CodTipoAvaliacao { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public string CodTurno { get; set; }
        public Nullable<int> NumTurma { get; set; }
        public Nullable<int> Periodo { get; set; }
        public Nullable<int> CodCurso { get; set; }
        public Nullable<int> CodSala { get; set; }
        public int CodProfessor { get; set; }
        public int CodDisciplina { get; set; }
        public Nullable<double> Valor { get; set; }
    
        public virtual Disciplina Disciplina { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual Sala Sala { get; set; }
        public virtual Avaliacao Avaliacao { get; set; }
        public virtual Turma Turma { get; set; }
    }
}
