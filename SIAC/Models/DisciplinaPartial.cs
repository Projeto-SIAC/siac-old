﻿using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Disciplina
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Disciplina> ListarOrdenadamente() => contexto.Disciplina.OrderBy(d => d.Descricao).ToList();

        public static Disciplina ListarPorCodigo(int codDisciplina) => contexto.Disciplina.Find(codDisciplina);

        public static int Inserir(Disciplina disciplina)
        {
            contexto.Disciplina.Add(disciplina);
            contexto.SaveChanges();
            return disciplina.CodDisciplina;
        }

        public static List<Disciplina> ListarTemQuestoes() =>
            contexto.QuestaoTema.Select(qt => qt.Tema.Disciplina)
            .Distinct()
            .OrderBy(d => d.Descricao)
            .ToList();

        public static List<Disciplina> ListarPorProfessor(string matrProfessor) => contexto.Professor.FirstOrDefault(p => p.MatrProfessor == matrProfessor).Disciplina.ToList();
    }
}