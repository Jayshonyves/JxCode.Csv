using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxCode.CSV
{
    public class CsvWriter
    {
        public static string Write(CsvData csvData)
        {
            StringBuilder sb = new StringBuilder();
            //header
            WriteLine(sb, csvData.Header);
            //body
            foreach (CsvRecord item in csvData)
            {
                WriteLine(sb, item);
            }
            string rtn = sb.ToString();
            sb.Clear();
            return rtn;
        }
        private static void WriteLine(StringBuilder sb, IEnumerable<string> strs)
        {
            int count = strs.Count();
            var it = strs.GetEnumerator();
            for (int i = 0; it.MoveNext(); i++)
            {
                sb.Append(Format(it.Current));
                //no last
                if (i != count - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append('\n');
        }

        private static string Format(string str)
        {
            if(str.IndexOf('"') != -1 || str.IndexOf(',') != -1)
            {
                return "\"" + str.Replace("\"", "\"\"") + "\"";
            }
            else
            {
                return str;
            }
        }
        
    }
}
