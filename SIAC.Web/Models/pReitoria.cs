﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Reitoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodReitoria}";

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Reitoria> ListarOrdenadamente()
        {
            return contexto.Reitoria.OrderBy(c => c.Sigla).ToList();
        }

        public static Reitoria ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codReitoria = int.Parse(codigos[1]);

            return contexto.Reitoria.FirstOrDefault(r => r.CodInstituicao == codInstituicao
                                                    && r.CodReitoria == codReitoria);
        }
    }
}