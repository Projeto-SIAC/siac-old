﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Aluno
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(Aluno aluno)
        {
            contexto.Aluno.Add(aluno);
            contexto.SaveChanges();
        }

        public static List<Aluno> ListarOrdenadamente()
        {
            return contexto.Aluno.OrderBy(a => a.Usuario.PessoaFisica.Nome).ToList();
        }


        public static Aluno ListarPorCodigo(int codAluno)
        {
            return contexto.Aluno.FirstOrDefault(a => a.CodAluno == codAluno);
        }

        public static Aluno ListarPorMatricula(string strMatricula)
        {
            return contexto.Aluno.FirstOrDefault(a => a.MatrAluno == strMatricula);
        }        
    }
}