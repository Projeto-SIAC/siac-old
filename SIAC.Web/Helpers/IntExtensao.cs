﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Web.Models;

namespace SIAC.Web
{
    public static class IntExtensao
    {
        public static string GetIndiceAlternativa(this int i)
        {
            i++;
            int tipo = DataContextSIAC.GetInstance().Parametro.First().NumeracaoAlternativa.Value;

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
            int tipo = DataContextSIAC.GetInstance().Parametro.ElementAt(0).NumeracaoQuestao.Value;

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
            const string letters = " abcdefghijklmnopqrstuvwxyz";

            string value = "";

            if (number >= letters.Length)
                value += letters[number/ letters.Length - 1];

            value += letters[number % letters.Length];

            return value;
        }

        public static string paraCaixaAlta(int number)
        {
            return paraCaixaBaixa(number).ToUpper();
        }
    }
}