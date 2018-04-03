using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MyNamespace.Utils;

namespace MyNamespace.CodeGenerator
{
    public static class CommonUtil
    {
        public const string NodeClassPattern = @"class(.*?):\sCustomNode";
        public const string GeneratedClassPattern = @"__GeneratedNode_(.*?)__";
        public const string NamespacePattern = @"using(.*?);";
        public const string MethodContentPattern = @"\s${method_name}\s*\(.*?\)\s*{((\s*.*?\s*)*?)}";
        public const string FunctionTemplate = "\t${function_header}\n\t{${function_body}\n\t}";

        public static Dictionary<string, string> FindAllClassPath(string fileRootPath)
        {
            var classDic = new Dictionary<string, string>();
            var classFiles = FileHelper.GetFiles(fileRootPath, "*.cs", true);

            foreach (var file in classFiles)
            {
                var content = FileHelper.Read(file.FullName);
                MatchCollection mc = Regex.Matches(content, NodeClassPattern);
                foreach (Match match in mc)
                {
                    var className = match.Groups[1].Value.Trim();
                    if (!Regex.IsMatch(className, GeneratedClassPattern))
                    {
                        classDic.Add(className, file.FullName);
                    }
                }
            }

            return classDic;
        }

        public static string GetNamespace(string content)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using MyNamespace.Runtime;");

            MatchCollection mc = Regex.Matches(content, NamespacePattern);
            foreach (Match match in mc)
            {
                sb.AppendLine(string.Format("using {0};", match.Groups[1].Value.Trim()));
            }

            if (!sb.ToString().Contains("using System;"))
            {
                sb.AppendLine("using System;");
            }

            if (!sb.ToString().Contains("using MyNamespace.Runtime;"))
            {
                sb.AppendLine("using MyNamespace.Runtime;");
            }

            return sb.ToString();
        }

        public static string GetFunction(MethodInfo methodInfo, string content)
        {
            var sb = new StringBuilder();
            var match = Regex.Match(content, MethodContentPattern.Replace("${method_name}", methodInfo.Name), RegexOptions.Multiline);
            if (match != null && match.Success)
            {
                var startIndex = match.Groups[1].Captures[0].Index;
                sb.AppendLine(FunctionTemplate
                            .Replace("${function_header}", GetFunctionHeader(startIndex, content))
                            .Replace("${function_body}", GetFunctionBody(startIndex, content)));
            }

            return sb.ToString();
        }

        public static string GetFunctionContent(MethodInfo methodInfo, string content)
        {
            if (methodInfo == null)
                return "";

            // todo 该用 code parse 生成代码，正则式无法处理括号嵌套情况

            var ret = string.Empty;

            var match = Regex.Match(content, MethodContentPattern.Replace("${method_name}", methodInfo.Name), RegexOptions.Multiline);
            if (match.Success)
            {
                ret = CommonUtil.GetFunctionBody(match.Groups[1].Captures[0].Index, content);
            }

            return ret;
        }

        public static string GetFunctionHeader(int startIndex, string content)
        {
            var sb = new StringBuilder();

            int currentIndex = startIndex;
            var ch = content[currentIndex];

            while (currentIndex - 1 >= 0 && ch != '{')
            {
                ch = content[--currentIndex];
            }
            currentIndex--;

            while (currentIndex >= 0)
            {
                ch = content[currentIndex--];

                if (ch == '}' || ch == ';')
                    break;

                sb.Insert(0, ch);
            }

            return sb.ToString().Trim();
        }

        public static string GetFunctionBody(int startIndex, string content)
        {
            var sb = new StringBuilder();

            int bracketNum = 1;
            int currentIndex = startIndex;

            while (bracketNum > 0 && currentIndex < content.Length)
            {
                var ch = content[currentIndex];

                switch (ch)
                {
                    case '{':
                        bracketNum++;
                        sb.Append(ch);
                        break;

                    case '}':
                        bracketNum--;
                        if (bracketNum != 0)
                            sb.Append(ch);

                        break;

                    default:
                        sb.Append(ch);
                        break;
                }

                currentIndex++;
            }

            return sb.ToString();
        }
    }
}

