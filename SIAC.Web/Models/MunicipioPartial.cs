﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Municipio
    {
        public static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Municipio> ListarOrdenadamente()
        {
            return contexto.Municipio.ToList();
        }

        public static List<Pais> ListarPaisesOrdenadamente()
        {
            return contexto.Municipio.Select(m => m.Estado.Pais).Distinct().OrderBy(p => p.Descricao).ToList();
        }

        public static List<Estado> ListarEstadosOrdenadamente()
        {
            return contexto.Municipio.Select(m => m.Estado).Distinct().OrderBy(e => e.Descricao).ToList();
        }

        public static Municipio ListarPorCodigo(int pais, int estado, int municipio)
        {
            return contexto.Municipio.Find(pais, estado, municipio);
        }
    }
}