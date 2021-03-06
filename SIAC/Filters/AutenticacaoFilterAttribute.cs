﻿/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using SIAC.Helpers;
using SIAC.Models;
using System.Linq;
using System.Web.Mvc;

namespace SIAC.Filters
{
    public class AutenticacaoFilterAttribute : ActionFilterAttribute
    {
        public int[] Categorias { get; set; }
        public int[] Ocupacoes { get; set; }
        public bool SomenteOcupacaoAvi { get; set; } = false;
        public bool SomenteOcupacaoSimulado { get; set; } = false;

        private int[] SimuladoOcupacoesPermitidas = {
            Ocupacao.SUPERUSUARIO,
            Ocupacao.REITOR,
            Ocupacao.DIRETOR_GERAL,
            Ocupacao.COORDENADOR_SIMULADO,
            Ocupacao.COLABORADOR_SIMULADO
        };

        private int[] AviOcupacoesPermitidas => Parametro.Obter().OcupacaoCoordenadorAvi;

        private ActionResult Redirecionar(ActionExecutingContext filterContext, string url = null)
        {
            if (!string.IsNullOrEmpty(Sessao.UsuarioMatricula))
            {
                Lembrete.AdicionarNotificacao("Você tentou acessar uma área sem permissão.", Lembrete.INFO);
            }
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                if (string.IsNullOrEmpty(url))
                {
                    return new RedirectResult("~/?continuar=" + filterContext.HttpContext.Request.Path);
                }
                else
                {
                    return new RedirectResult(url);
                }
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool autenticado = Sistema.Autenticado(Sessao.UsuarioMatricula);

            if (!autenticado)
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (Sessao.UsuarioCategoriaCodigo == Categoria.VISITANTE && Sessao.UsuarioSenhaPadrao)
            {
                filterContext.Result = Redirecionar(filterContext, "~/acesso/visitante");
            }
            else if (Categorias != null && !Categorias.Contains(Sessao.UsuarioCategoriaCodigo))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (Ocupacoes != null && !Ocupacoes.ContainsOne(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodOcupacao))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (SomenteOcupacaoAvi && !AviOcupacoesPermitidas.ContainsOne(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodOcupacao))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (SomenteOcupacaoSimulado && !SimuladoOcupacoesPermitidas.ContainsOne(Sistema.UsuarioAtivo[Sessao.UsuarioMatricula].Usuario.CodOcupacao))
            {
                filterContext.Result = Redirecionar(filterContext);
            }
            else if (Sessao.RealizandoAvaliacao)
            {
                string[] paths = filterContext.HttpContext.Request.Path.ToLower().Split('/');
                if (paths.Length > 0)
                {
                    string codigo = paths[paths.Length - 1];
                    if (paths.Contains("realizar"))
                    {
                        if (Sistema.AvaliacaoUsuario.ContainsKey(codigo))
                        {
                            if (Sistema.AvaliacaoUsuario[codigo].Contains(Sessao.UsuarioMatricula))
                            {
                                filterContext.Result = new RedirectResult("~/erro/1");
                            }
                        }
                    }
                    else if (filterContext.HttpContext.Request.HttpMethod == "GET")
                    {
                        filterContext.Result = new RedirectResult("~/erro/1");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/erro/1");
                }
            }

            // usuario autorizado
            base.OnActionExecuting(filterContext);
        }
    }
}