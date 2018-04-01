using System;
using System.Collections.Generic;
using System.Text;

namespace MyNamespace.Utils
{

    public static class StringHelper
    {
        public static string ListToStr<T>(List<T> list, string separator = "\n")
        {
            if (list == null || list.Count == 0)
                return "";

            var sb = new StringBuilder();

            for (int i = 0, count = list.Count - 1; i < count; i++)
            {
                sb.Append(list[i] + "").Append(separator);
            }
            sb.Append(list[list.Count - 1]);

            return sb.ToString();
        }

        public static string StrToLegalVarName(string name)
        {
            var sb = new StringBuilder();
            for (int i = 0, len = name.Length; i < len; i++)
            {
                switch (name[i])
                {
                    case '.':
                        sb.Append("___");
                        break;

                    case '[':
                        sb.Append("_0_");
                        break;

                    case ']':
                        sb.Append("__0");
                        break;

                    default:
                        sb.Append(name[i]);
                        break;
                }
            }

            return sb.ToString();
        }

        public static string VarNameToTypeStr(string name)
        {
            var sb = new StringBuilder();
            int i = 0;
            int len = name.Length;
            var s = "";

            while (i < len - 2)
            {
                if (name[i] != '_')
                {
                    sb.Append(name[i++]);
                }
                else
                {
                    s = name.Substring(i, 3);
                    switch (s)
                    {
                        case "___":
                            sb.Append('.');
                            i += 3;
                            continue;

                        case "_0_":
                            sb.Append('[');
                            i += 3;
                            continue;

                        case "__0":
                            sb.Append(']');
                            i += 3;
                            continue;
                    }

                    sb.Append(name[i++]);
                }
            }

            if (i < len - 1)
            {
                sb.Append(name.Substring(i, len - 1 - i));
            }

            return sb.ToString();
        }
    }
}