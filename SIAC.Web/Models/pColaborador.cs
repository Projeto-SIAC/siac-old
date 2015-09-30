﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Colaborador
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static void Inserir(Colaborador colaborador)
        {
            contexto.Colaborador.Add(colaborador);
            contexto.SaveChanges();
        }

        public static List<Colaborador> ListarOrdenadamente()
        {
            return contexto.Colaborador.OrderBy(c => c.Usuario.PessoaFisica.Nome).ToList();
        }
    }
}