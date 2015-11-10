﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Campus
    {
        public string CodComposto => $"{CodInstituicao}.{CodCampus}";

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Campus> ListarOrdenadamente()
        {
            return contexto.Campus.OrderBy(c => c.Sigla).ToList();
        }

        public static void Inserir(Campus campus)
        {
            contexto.Campus.Add(campus);
            contexto.SaveChanges();
        }

        public static Campus ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);

            return contexto.Campus.FirstOrDefault(c => c.CodInstituicao == codInstituicao
                                                    && c.CodCampus == codCampus);
        }
    }
}