﻿using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC
{
    public static class Extensoes
    {
        #region byte[]
        public static string GetString(this byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        public static string GetImageSource(this byte[] bytes)
        {
            //<img src="data:image/png;base64,@Convert.ToBase64String(Model.Anexo, 0, Model.Anexo.Length)"/>
            string src = "data:image/png;base64,";
            src += Convert.ToBase64String(bytes, 0, bytes.Length);
            return src;
        }
        #endregion

        // String
        #region String
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string RemoveSpaces(this string aText)
        {
            aText = aText.Replace("\t", " ");
            aText = aText.Replace("\n", " ");
            aText = aText.Replace("\r", " ");
            string result = aText.Trim();
            while (result.IndexOf("  ") > 0)
            {
                result = result.Replace("  ", " ");
            }
            return result;
        }

        public static string ToShortString(this string str, int length)
        {
            string text = string.Empty;

            if (str.Length > length)
            {
                text = str.Substring(0, length);
                string afterText = str.Substring(length);

                if (afterText.IndexOf(' ') > -1)
                {
                    afterText = afterText.Remove(afterText.IndexOf(' '));

                    afterText += "...";
                }
                else
                {
                    afterText = "...";
                }

                text += afterText;

            }
            else
            {
                text = str;
            }

            return text;
        }

        public static string ToHtml(this string str, params string[] tags)
        {
            string html = String.Empty;

            var strs = str.Split('\n', '\r');
            foreach (var p in strs)
            {
                if (!String.IsNullOrWhiteSpace(p))
                {
                    tags = tags.Reverse().ToArray();

                    var text = p;

                    foreach (var tag in tags)
                    {
                        text = $"<{tag}>{text}</{tag}>";
                    }

                    html += text;
                }
            }

            return html;
        }

        #endregion

        // Int
        #region Int
        public static string GetIndiceAlternativa(this int i)
        {
            i++;
            int tipo = Parametro.Obter().NumeracaoAlternativa;

            switch (tipo)
            {
                case 1:
                    return i.ToString();
                case 2:
                    return paraRomano(i);
                case 3:
                    return paraCaixaBaixa(i);
                case 4:
                    return paraCaixaAlta(i);
                default:
                    return paraCaixaBaixa(i);
            }
        }

        public static string GetIndiceQuestao(this int i)
        {
            i++;
            int tipo = Parametro.Obter().NumeracaoQuestao;

            switch (tipo)
            {
                case 1:
                    return i.ToString();
                case 2:
                    return paraRomano(i);
                case 3:
                    return paraCaixaBaixa(i);
                case 4:
                    return paraCaixaAlta(i);
                default:
                    return paraCaixaBaixa(i);
            }
        }

        public static string paraRomano(int number)
        {
            if ((number < 0) || (number > 3999)) return number.ToString();
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + paraRomano(number - 1000);
            if (number >= 900) return "CM" + paraRomano(number - 900);
            if (number >= 500) return "D" + paraRomano(number - 500);
            if (number >= 400) return "CD" + paraRomano(number - 400);
            if (number >= 100) return "C" + paraRomano(number - 100);
            if (number >= 90) return "XC" + paraRomano(number - 90);
            if (number >= 50) return "L" + paraRomano(number - 50);
            if (number >= 40) return "XL" + paraRomano(number - 40);
            if (number >= 10) return "X" + paraRomano(number - 10);
            if (number >= 9) return "IX" + paraRomano(number - 9);
            if (number >= 5) return "V" + paraRomano(number - 5);
            if (number >= 4) return "IV" + paraRomano(number - 4);
            if (number >= 1) return "I" + paraRomano(number - 1);

            return number.ToString();
        }

        public static string paraCaixaBaixa(int number)
        {
            number--;
            const string letters = "abcdefghijklmnopqrstuvwxyz";

            string value = "";

            if (number >= letters.Length)
                value += letters[number / letters.Length - 1];

            value += letters[number % letters.Length];

            return value;
        }

        public static string paraCaixaAlta(int number) => paraCaixaBaixa(number).ToUpper();

        public static bool ContainsOne(this int[] i, int[] j)
        {
            foreach (var k in j)
            {
                if (i.Contains(k))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        // DateTime
        #region DateTime
        public static string ToBrazilianString(this DateTime dateTime)
        {
            return dateTime.ToString("dddd, dd 'de' MMMM 'de' yyyy 'às' HH'h'mm", new System.Globalization.CultureInfo("pt-BR"));
        }

        public static bool IsFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now ? true : false;
        }

        public static string ToElapsedTimeString(this DateTime dt)
        {
            string s = String.Empty;
            double segundos = (DateTime.Now - dt).TotalSeconds;
            if (segundos < 1)
            {
                s = "Agora a pouco";
            }
            else if (segundos < 60)
            {
                double q = Math.Round(segundos);
                s = q > 1 ? q + " segundos atrás" : q + " segundo atrás";
            }
            else if (segundos < 3600)
            {
                double q = Math.Round(segundos / 60);
                s = q > 1 ? q + " minutos atrás" : q + " minuto atrás";
            }
            else if (segundos < 86400)
            {
                double q = Math.Round((segundos / 60) / 60);
                s = q > 1 ? q + " horas atrás" : q + " hora atrás";
            }
            else if (segundos < (86400 * 15))
            {
                double q = Math.Round(((segundos / 60) / 60) / 24);
                s = q > 1 ? q + " dias atrás" : q + " dia atrás";
            }
            else
            {
                s = dt.ToShortDateString();
            }
            return s;
        }

        public static string ToLeftTimeString(this DateTime dt)
        {
            string s = String.Empty;
            double segundos = (dt - DateTime.Now).TotalSeconds;
            if (segundos < 1)
            {
                s = "Agora";
            }
            else if (segundos < 60)
            {
                double q = Math.Round(segundos);
                s = q > 1 ? q + " segundos" : q + " segundo";
            }
            else if (segundos < 3600)
            {
                double q = Math.Round(segundos / 60);
                s = q > 1 ? q + " minutos" : q + " minuto";
            }
            else if (segundos < 86400)
            {
                double q = Math.Round((segundos / 60) / 60);
                s = q > 1 ? q + " horas" : q + " hora";
            }
            else
            {
                double q = Math.Round(((segundos / 60) / 60) / 24);
                s = q > 1 ? q + " dias" : q + " dia";
            }
            return s;
        }

        public static int SemestreAtual(this DateTime dt) => dt.Month > 6 ? 2 : 1;
        #endregion

        // List<AvaliacaoTema>
        #region List<AvaliacaoTema>
        public static int QteQuestoesPorTipo(this List<AvaliacaoTema> lstAvaliacaoTema, int codDisciplina, int codTipoQuestao)
        {
            int qteQuestoes = 0;

            foreach (var avaliacaoTema in lstAvaliacaoTema.Where(a => a.Tema.CodDisciplina == codDisciplina))
            {
                var lstAvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.ToList();
                var lstAvalTemaQuestaoFiltrada = lstAvalTemaQuestao.Where(a => a.QuestaoTema.Questao.CodTipoQuestao == codTipoQuestao).ToList();
                qteQuestoes += lstAvalTemaQuestaoFiltrada.Count;
            }

            return qteQuestoes;
        }

        public static int QteQuestoesPorTipo(this List<AvaliacaoTema> lstAvaliacaoTema, int codTipoQuestao)
        {
            int qteQuestoes = 0;

            foreach (var avaliacaoTema in lstAvaliacaoTema)
            {
                var lstAvalTemaQuestao = avaliacaoTema.AvalTemaQuestao.ToList();
                var lstAvalTemaQuestaoFiltrada = lstAvalTemaQuestao.Where(a => a.QuestaoTema.Questao.CodTipoQuestao == codTipoQuestao).ToList();
                qteQuestoes += lstAvalTemaQuestaoFiltrada.Count;
            }

            return qteQuestoes;
        }

        public static string MaxDificuldade(this List<AvaliacaoTema> lstAvaliacaoTema, int codDisciplina)
        {
            Dificuldade dificuldade = new Dificuldade();

            foreach (var avaliacaoTema in lstAvaliacaoTema.Where(a => a.Tema.CodDisciplina == codDisciplina))
            {
                var lstDificuldade = avaliacaoTema.AvalTemaQuestao.Select(a => a.QuestaoTema.Questao.Dificuldade).ToList();
                if (lstDificuldade.Count > 0 && lstDificuldade.Max(a => a.CodDificuldade) > dificuldade.CodDificuldade)
                {
                    dificuldade = lstDificuldade.First(a => a.CodDificuldade == lstDificuldade.Max(d => d.CodDificuldade));
                }
            }

            return dificuldade.Descricao;
        }
        #endregion

        // Avaliacao
        #region Avaliacao
        public static int QteQuestoes(this Avaliacao avaliacao)
        {
            int qte = 0;

            foreach (var avalTema in avaliacao.AvaliacaoTema)
            {
                qte += avalTema.AvalTemaQuestao.Count;
            }

            return qte;
        }

        public static List<Questao> EmbaralharQuestao(this Avaliacao avaliacao)
        {
            List<Questao> lstQuestao = new List<Models.Questao>();
            List<Questao> lstQuestaoEmbalharada = new List<Questao>();
            Random r = new Random();

            foreach (var avalTema in avaliacao.AvaliacaoTema)
            {
                lstQuestao.AddRange(avalTema.AvalTemaQuestao.Select(a => a.QuestaoTema.Questao).ToList());
            }

            while (lstQuestao.Count > 0)
            {
                int i = r.Next(lstQuestao.Count);
                Questao qst = lstQuestao.ElementAt(i);
                lstQuestaoEmbalharada.Add(qst);
                lstQuestao.Remove(qst);
            }

            return lstQuestaoEmbalharada;
        }
        #endregion

        // Questao
        #region Questao
        public static bool TemUmaCorreta(this Questao questao)
        {
            bool unica = false;

            foreach (var alt in questao.Alternativa)
            {
                if (alt.FlagGabarito.HasValue && alt.FlagGabarito.Value)
                {
                    if (unica == true)
                    {
                        unica = false;
                        break;
                    }
                    unica = true;
                }
            }

            return unica;
        }

        public static List<Alternativa> EmbaralharAlternativa(this Questao questao)
        {
            List<Alternativa> lstAlternativa = questao.Alternativa.ToList();
            List<Alternativa> lstAlternativaEmbaralhada = new List<Alternativa>();
            Random r = new Random();

            while (lstAlternativaEmbaralhada.Count != questao.Alternativa.Count)
            {
                int i = r.Next(lstAlternativa.Count);
                Alternativa alt = lstAlternativa.ElementAt(i);
                lstAlternativaEmbaralhada.Add(alt);
                lstAlternativa.Remove(alt);
            }

            return lstAlternativaEmbaralhada;
        }

        public static List<AvalQuesPessoaResposta> Resposta(this Questao questao)
        {
            List<AvalQuesPessoaResposta> lstResposta = new List<AvalQuesPessoaResposta>();

            lstResposta.AddRange(
                    from r in Repositorio.GetInstance().AvalQuesPessoaResposta
                    where r.CodQuestao == questao.CodQuestao
                    select r
                );

            return lstResposta;
        }

        public static string ToJsonChart(this Questao questao, List<AvalQuesPessoaResposta> lstResposta)
        {
            var random = new Random();
            string json = string.Empty;
            json += "[";

            for (int i = 0, length = questao.Alternativa.Count; i < length; i++)
            {
                string rgba = Helpers.CorDinamica.Rgba(random);

                json += "{";

                json += $"\"value\":\"{lstResposta.Where(r => r.RespAlternativa == i).Count()}\"";

                json += ",";

                json += $"\"label\":\"Alternativa {i.GetIndiceAlternativa()}\"";

                json += ",";

                json += $"\"color\":\"{rgba}\"";

                json += ",";

                json += $"\"highlight\":\"{rgba.Replace("1)", "0.8)")}\"";

                json += "}";

                if (i != length - 1)
                {
                    json += ",";
                }
            }

            json += "]";

            return json;
        }

        #endregion

        //Double
        #region Double
        public static string ToValueHtml(this double value) => value.ToString().Replace(',', '.');
        #endregion

        public static string RecuperarIp(this HttpContext contexto)
        {
            string ip = contexto.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ip))
            {
                string[] enderecos = ip.Split(',');
                if (enderecos.Length != 0)
                {
                    return enderecos[0];
                }
            }

            return contexto.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}