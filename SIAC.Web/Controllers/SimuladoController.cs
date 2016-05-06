﻿using SIAC.Helpers;
using SIAC.Models;
using SIAC.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIAC.Controllers
{
    [Filters.AutenticacaoFilter(Categorias = new[] { Categoria.COLABORADOR })]
    public class SimuladoController : Controller
    {
        private dbSIACEntities contexto => Repositorio.GetInstance();

        public ActionResult Novo() => View();

        [HttpPost]
        public ActionResult Novo(FormCollection form)
        {
            if (!StringExt.IsNullOrWhiteSpace(form["txtTitulo"]))
            {
                Simulado sim = new Simulado();
                DateTime hoje = DateTime.Now;
                /* Chave */
                sim.Ano = hoje.Year;
                sim.NumIdentificador = Simulado.ObterNumIdentificador();
                sim.DtCadastro = hoje;

                /* Simulado */
                sim.Titulo = form["txtTitulo"];
                sim.Descricao = form["txtDescricao"];
                sim.FlagInscricaoEncerrado = true;

                /* Colaborador */
                sim.Colaborador = Colaborador.ListarPorMatricula(Sessao.UsuarioMatricula);

                Simulado.Inserir(sim);
                Lembrete.AdicionarNotificacao("Simulado cadastrado com sucesso.", Lembrete.POSITIVO);
                return RedirectToAction("Provas", new { codigo = sim.Codigo });
            }
            Lembrete.AdicionarNotificacao("Ocorreu um erro na operação.", Lembrete.NEGATIVO);
            return RedirectToAction("Novo");
        }

        [HttpPost]
        public ActionResult Editar(string codigo, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string titulo = form["txtTitulo"];
                    string descricao = form["txtDescricao"];

                    if (!StringExt.IsNullOrWhiteSpace(titulo))
                    {
                        sim.Titulo = titulo;
                        sim.Descricao = descricao;

                        Repositorio.Commit();

                        mensagem = "Simulado editado com sucesso.";
                        estilo = Lembrete.POSITIVO;
                    }
                    else
                    {
                        mensagem = "O campo de Título é obrigatório.";
                        estilo = Lembrete.NEGATIVO;
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem,estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        public ActionResult Provas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    return View(new SimuladoProvaViewModel()
                    {
                        Simulado = sim,
                        Disciplinas = Disciplina.ListarOrdenadamente()
                    });
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        public ActionResult Datas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    return View(sim);
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        [HttpPost]
        public ActionResult Datas(string codigo, FormCollection form)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string inicioInscricao = form["txtInicioInscricao"];
                    string terminoInscricao = form["txtTerminoInscricao"];
                    string qteVagas = form["txtQteVagas"];
                    if (!StringExt.IsNullOrWhiteSpace(inicioInscricao, terminoInscricao, qteVagas))
                    {
                        DateTime inicio = DateTime.Parse(inicioInscricao, new CultureInfo("pt-BR"));
                        DateTime termino = DateTime.Parse($"{terminoInscricao} 23:59:59", new CultureInfo("pt-BR"));
                        
                        if (sim.DtInicioInscricao <= DateTime.Now)
                        {
                            Lembrete.AdicionarNotificacao("As incrições já foram iniciadas, você não pode mais alterar o início.", Lembrete.NEGATIVO);
                            return View(sim);
                        }

                        /* Simulado */
                        sim.QteVagas = int.Parse(qteVagas);
                        sim.DtInicioInscricao = inicio;
                        sim.DtTerminoInscricao = termino;

                        Repositorio.GetInstance().SaveChanges();

                        Lembrete.AdicionarNotificacao("As datas foram atualizadas.", Lembrete.POSITIVO);

                        return RedirectToAction("Salas", new { codigo = sim.Codigo });
                    }
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        public ActionResult Salas(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimuladoSalasViewModel model = new SimuladoSalasViewModel();
                    model.Simulado = sim;
                    model.Campi = Campus.ListarOrdenadamente();
                    model.Blocos = Bloco.ListarOrdenadamente();
                    model.Salas = Sala.ListarOrdenadamente();

                    return View(model);
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        [HttpPost]
        public ActionResult Salas(string codigo, FormCollection form)
        {
            string ddlCampus = form["ddlCampus"];
            string ddlBloco = form["ddlBloco"];
            string ddlSala = form["ddlSala"];

            if (!StringExt.IsNullOrWhiteSpace(codigo, ddlCampus, ddlBloco, ddlSala))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    int codSala = int.Parse(ddlSala);

                    SimSala simSala = contexto.SimSala.FirstOrDefault(s => s.Ano == sim.Ano
                                                                      && s.NumIdentificador == sim.NumIdentificador
                                                                      && s.CodSala == codSala);
                    if (simSala == null)
                    {
                        sim.SimSala.Add(new SimSala()
                        {
                            Sala = Sala.ListarPorCodigo(int.Parse(ddlSala))
                        });

                        Repositorio.Commit();
                    }
                    Lembrete.AdicionarNotificacao("A sala foi adicionada com sucesso.", Lembrete.POSITIVO);
                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }
            Lembrete.AdicionarNotificacao("Ocorreu um erro na operação.", Lembrete.NEGATIVO);
            return RedirectToAction("Salas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverSala(string codigo, int codSala)
        {
            if (!StringExt.IsNullOrWhiteSpace(codigo, codSala.ToString()))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimSala simSala = contexto.SimSala
                        .FirstOrDefault(s => s.Ano == sim.Ano && s.NumIdentificador == sim.NumIdentificador && s.CodSala == codSala);

                    sim.SimSala.Remove(simSala);
                    Repositorio.Commit();

                    Lembrete.AdicionarNotificacao("A sala foi removida com sucesso.", Lembrete.POSITIVO);
                    return RedirectToAction("Salas", new { codigo = codigo });
                }
            }

            Lembrete.AdicionarNotificacao("Ocorreu um erro na operação.", Lembrete.NEGATIVO);
            return RedirectToAction("Salas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarDia(string codigo, int codDia)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao == null) diaRealizacao = new SimDiaRealizacao();

                    return PartialView("_SimuladoDiaCarregar", diaRealizacao);
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult NovoDia(string codigo, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string strDataRealizacao = form["txtDataRealizacao"];
                    string strHorarioInicio = form["txtHorarioInicio"];
                    string strHorarioTermino = form["txtHorarioTermino"];

                    if (!StringExt.IsNullOrWhiteSpace(strDataRealizacao, strHorarioInicio, strHorarioTermino))
                    {
                        CultureInfo cultureBr = new CultureInfo("pt-BR");
                        /* Simulado */
                        DateTime dataRealizacao = DateTime.Parse($"{strDataRealizacao} {strHorarioInicio}", cultureBr);
                        TimeSpan inicio = TimeSpan.Parse(strHorarioInicio, cultureBr);
                        TimeSpan termino = TimeSpan.Parse(strHorarioTermino, cultureBr);

                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.DtRealizacao.Date == dataRealizacao.Date);

                        if (diaRealizacao != null && inicio >= diaRealizacao.DtRealizacao.TimeOfDay && inicio <= diaRealizacao.DtRealizacao.AddMinutes(diaRealizacao.Duracao).TimeOfDay)
                        {
                            mensagem = $"Já existe uma data marcada com a realização nesse período: {dataRealizacao.ToShortDateString()} ({inicio.ToString("HH:mm")} até {termino.ToString("HH:mm")}).";
                            estilo = Lembrete.NEGATIVO;
                        }
                        else
                        {
                            int codDiaRealizacao = sim.SimDiaRealizacao.Count > 0 ? sim.SimDiaRealizacao.Max(s => s.CodDiaRealizacao) + 1 : 1;

                            diaRealizacao = new SimDiaRealizacao();
                            diaRealizacao.CodDiaRealizacao = codDiaRealizacao;
                            diaRealizacao.DtRealizacao = dataRealizacao;
                            diaRealizacao.CodTurno = "V";
                            diaRealizacao.Duracao = int.Parse((termino - dataRealizacao.TimeOfDay).TotalMinutes.ToString());

                            sim.SimDiaRealizacao.Add(diaRealizacao);
                            Repositorio.GetInstance().SaveChanges();

                            mensagem = "O dia foi adicionado com sucesso.";
                            estilo = Lembrete.POSITIVO;
                        }
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult EditarDia(string codigo, int codDia, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string strDataRealizacao = form["txtDataRealizacao"];
                    string strHorarioInicio = form["txtHorarioInicio"];
                    string strHorarioTermino = form["txtHorarioTermino"];

                    if (!StringExt.IsNullOrWhiteSpace(strDataRealizacao, strHorarioInicio, strHorarioTermino))
                    {
                        CultureInfo cultureBr = new CultureInfo("pt-BR");
                        /* Simulado */
                        DateTime dataRealizacao = DateTime.Parse($"{strDataRealizacao} {strHorarioInicio}", cultureBr);
                        TimeSpan inicio = TimeSpan.Parse(strHorarioInicio, cultureBr);
                        TimeSpan termino = TimeSpan.Parse(strHorarioTermino, cultureBr);

                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.DtRealizacao.Date == dataRealizacao.Date && s.CodDiaRealizacao != codDia);

                        if (diaRealizacao != null && inicio >= diaRealizacao.DtRealizacao.TimeOfDay && inicio <= diaRealizacao.DtRealizacao.AddMinutes(diaRealizacao.Duracao).TimeOfDay)
                        {
                            mensagem = $"Já existe uma data marcada com a realização nesse período: {dataRealizacao.ToShortDateString()} ({inicio.ToString("HH:mm")} até {termino.ToString("HH:mm")}).";
                            estilo = Lembrete.NEGATIVO;
                        }
                        else
                        {
                            int codDiaRealizacao = sim.SimDiaRealizacao.Count > 0 ? sim.SimDiaRealizacao.Max(s => s.CodDiaRealizacao) + 1 : 1;

                            diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(d => d.CodDiaRealizacao == codDia);
                            diaRealizacao.DtRealizacao = dataRealizacao;
                            diaRealizacao.CodTurno = "V";
                            diaRealizacao.Duracao = int.Parse((termino - dataRealizacao.TimeOfDay).TotalMinutes.ToString());

                            Repositorio.Commit();

                            mensagem = "O dia foi editado com sucesso.";
                            estilo = Lembrete.POSITIVO;
                        }
                    }
                }
            }
            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverDia(string codigo, int codDia)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    sim.SimDiaRealizacao.Remove(diaRealizacao);
                    Repositorio.GetInstance().SaveChanges();

                    mensagem = "O dia foi removido com sucesso.";
                    estilo = Lembrete.POSITIVO;
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarProvas(string codigo, int codDia)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    return PartialView("_DiaProvas", diaRealizacao);
                }
            }

            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult CarregarProva(string codigo, int codDia, int codProva)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao != null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);

                        if (prova == null) prova = new SimProva();

                        return PartialView("_SimuladoProvaCarregar", new SimuladoProvaViewModel()
                        {
                            Simulado = sim,
                            Prova = prova,
                            Disciplinas = Disciplina.ListarOrdenadamente()
                        });
                    }
                }
            }
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult NovaProva(string codigo, int codDia, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string ddlDisciplina = form["ddlDisciplina"];
                    string txtQteQuestoes = form["txtQteQuestoes"];
                    string txtTitulo = form["txtTitulo"];
                    string txtDescricao = form["txtDescricao"];

                    if (!StringExt.IsNullOrWhiteSpace(ddlDisciplina, txtQteQuestoes, txtTitulo))
                    {
                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                        SimProva prova = new SimProva();

                        prova.CodProva = diaRealizacao.SimProva.Count > 0 ? diaRealizacao.SimProva.Max(p => p.CodProva) + 1 : 1;
                        prova.Titulo = txtTitulo;
                        prova.Descricao = String.IsNullOrWhiteSpace(txtDescricao) ? String.Empty : txtDescricao;
                        prova.QteQuestoes = int.Parse(txtQteQuestoes);
                        prova.CodDisciplina = int.Parse(ddlDisciplina);

                        List<Questao> questoes = Simulado.ObterQuestoes(prova.CodDisciplina, prova.QteQuestoes);

                        prova.SimProvaQuestao.Clear();

                        foreach (Questao questao in questoes)
                        {
                            prova.SimProvaQuestao.Add(new SimProvaQuestao()
                            {
                                Questao = questao
                            });
                        }

                        if (questoes.Count >= prova.QteQuestoes)
                        {
                            diaRealizacao.SimProva.Add(prova);
                            Repositorio.Commit();

                            mensagem = "Prova cadastrada com sucesso neste simulado.";
                            estilo = Lembrete.POSITIVO;

                        }
                        else
                        {
                            mensagem = "Foi gerada uma quantidade menor de questões para a prova deste simulado.";
                            estilo = Lembrete.NEGATIVO;
                        }
                    }
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult EditarProva(string codigo, int codDia, int codProva, FormCollection form)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    string ddlDisciplina = form["ddlDisciplina"];
                    string txtQteQuestoes = form["txtQteQuestoes"];
                    string txtTitulo = form["txtTitulo"];
                    string txtDescricao = form["txtDescricao"];

                    if (!StringExt.IsNullOrWhiteSpace(ddlDisciplina, txtQteQuestoes, txtTitulo))
                    {
                        SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);
                        
                        prova.Titulo = txtTitulo;
                        prova.Descricao = String.IsNullOrWhiteSpace(txtDescricao) ? String.Empty : txtDescricao;
                        prova.QteQuestoes = int.Parse(txtQteQuestoes);
                        prova.CodDisciplina = int.Parse(ddlDisciplina);

                        List<Questao> questoes = Simulado.ObterQuestoes(prova.CodDisciplina, prova.QteQuestoes);

                        prova.SimProvaQuestao.Clear();

                        foreach (Questao questao in questoes)
                        {
                            prova.SimProvaQuestao.Add(new SimProvaQuestao()
                            {
                                Questao = questao
                            });
                        }

                        if (questoes.Count >= prova.QteQuestoes)
                        {
                            diaRealizacao.SimProva.Add(prova);
                            Repositorio.Commit();
                            
                            mensagem = "Prova editada com sucesso neste simulado.";
                            estilo = Lembrete.POSITIVO;
                        }
                        else
                        {
                            mensagem = "Foi gerada uma quantidade menor de questões para a prova deste simulado.";
                            estilo = Lembrete.NEGATIVO;
                        }
                    }
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult RemoverProva(string codigo, int codDia, int codProva)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula && !sim.FlagSimuladoEncerrado)
                {
                    SimDiaRealizacao diaRealizacao = sim.SimDiaRealizacao.FirstOrDefault(s => s.CodDiaRealizacao == codDia);

                    if (diaRealizacao!= null)
                    {
                        SimProva prova = diaRealizacao.SimProva.FirstOrDefault(p => p.CodProva == codProva);
                        prova.SimProvaQuestao.Clear();
                        diaRealizacao.SimProva.Remove(prova);
                        Repositorio.Commit();

                        mensagem = "A prova foi removida com sucesso.";
                        estilo = Lembrete.POSITIVO;
                    }
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Provas", new { codigo = codigo });
        }

        public ActionResult Detalhe(string codigo)
        {
            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    return View(sim);
                }
            }

            return RedirectToAction("", "Arquivo");
        }

        [HttpPost]
        public ActionResult Encerrar(string codigo)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    sim.FlagSimuladoEncerrado = true;
                    Repositorio.Commit();
                    mensagem = "O simulado foi encerrado com sucesso.";
                    estilo = Lembrete.INFO;
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }

        [HttpPost]
        public ActionResult AlterarPermissaoInscricao(string codigo, string acao)
        {
            string mensagem = "Ocorreu um erro na operação.";
            string estilo = Lembrete.NEGATIVO;

            if (!String.IsNullOrWhiteSpace(codigo))
            {
                Simulado sim = Simulado.ListarPorCodigo(codigo);

                if (sim != null && sim.Colaborador.MatrColaborador == Sessao.UsuarioMatricula)
                {
                    switch (acao)
                    {
                        case "Bloquear":
                            sim.FlagInscricaoEncerrado = true;
                            mensagem = "As inscrições foram bloqueadas com sucesso.";
                            estilo = Lembrete.NEGATIVO;
                            break;
                        case "Liberar":
                            if (sim.CadastroCompleto)
                            {
                                sim.FlagInscricaoEncerrado = false;
                                mensagem = "As inscrições foram liberadas com sucesso.";
                                estilo = Lembrete.POSITIVO;
                            }
                            else
                            {
                                mensagem = "É necessário terminar o cadastro do simulado.";
                                estilo = Lembrete.NEGATIVO;
                            }
                            break;
                        default:
                            break;
                    }
                    Repositorio.Commit();
                    
                }
            }

            Lembrete.AdicionarNotificacao(mensagem, estilo);
            return RedirectToAction("Detalhe", new { codigo = codigo });
        }
    }
}