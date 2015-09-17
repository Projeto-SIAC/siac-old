﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web
{
    public static class StringExtensao
    {
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

            if (str.Length > length) { 
                text = str.Substring(0, length);
                string afterText = str.Substring(length);

                if (afterText.IndexOf(' ') > -1)
                {
                    afterText = afterText.Remove(afterText.IndexOf(' '));

                    afterText += "...";
                }

                text += afterText;
                
            }
            else
            {
                text = str;
            }

            return text;
        }
    }
}