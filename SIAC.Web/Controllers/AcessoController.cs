﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAC.Web.Models;

namespace SIAC.Web.Controllers
{
    public class AcessoController : Controller
    {
        // GET: Acesso
        public ActionResult Index()
        {
            if (Session["Autenticado"] != null && !String.IsNullOrEmpty(Session["Autenticado"].ToString()))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        // GET: Acesso/Entrar
        [HttpGet]
        public ActionResult Entrar()
        {
            if (Session["Autenticado"] != null && !String.IsNullOrEmpty(Session["Autenticado"].ToString()))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Acao = "$('.first.modal').modal('show')";
            return View("Index");
        }

        // POST: Acesso/Entrar
        [HttpPost]
        public ActionResult Entrar(FormCollection formCollection)
        {
            bool valido = false;

            if (formCollection.HasKeys())
            {
                int categoria = 0;
                int.TryParse(formCollection["DropDownCategoria"].ToString(), out categoria);
                ViewBag.DropDownCategoria = categoria.ToString();

                if (!String.IsNullOrWhiteSpace(formCollection["TextBoxMatricula"]) && !String.IsNullOrWhiteSpace(formCollection["TextBoxSenha"]))
                {
                    string matricula = formCollection["TextBoxMatricula"].ToString();
                    string senha = formCollection["TextBoxSenha"].ToString();

                    ViewBag.TextBoxMatricula = matricula;

                    Usuario usuario = Usuario.Autenticar(matricula, senha);

                    if (usuario != null)
                    {
                        valido = true;
                        Session["Autenticado"] = true;
                        Session["UsuarioNome"] = usuario.PessoaFisica.Nome;
                        Session["UsuarioCategoria"] = usuario.Categoria.Descricao;
                    }
                }
            }

            if (valido)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Acao = "$('.second.modal').modal('show')";
                ViewBag.Erro = "error";
                return View("Index");
            }
        }

        // GET: Acesso/Sair
        public ActionResult Sair()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}