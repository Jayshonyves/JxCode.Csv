using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;

namespace JxCode.CSV
{
    public class CsvReader
    {
        private const char NUL = '\0';
        private const char CR = '\r';
        private const char LF = '\n';
        private const string LFSTR = "\n";

        private static int curLine = 1;

        private static string csvContent;
        //总索引
        private static int curPos = -1;
        //行字符索引，用来抛异常
        private static int curCharPos = 0;
        private static char separator = ',';

        private static char curChar
        {
            get
            {
                if (curPos > 0 && curPos < csvContent.Length)
                {
                    return csvContent[curPos];
                }
                else
                {
                    return NUL;
                }
            }
        }

        private static char GetChar()
        {
            if (curPos + 1 < csvContent.Length)
            {
                curPos++;
                curCharPos++;
                char c = csvContent[curPos];
                if (c == LF)
                {
                    curCharPos = 0;
                    curLine++;
                }
                return csvContent[curPos];
            }
            else
            {
                return NUL;
            }
        }
        private static char Peek(int index = 1)
        {
            if (curPos + index < csvContent.Length)
            {
                return csvContent[curPos + index];
            }
            else
            {
                return NUL;
            }
        }
        

        private static void Reset()
        {
            curLine = 1;
            csvContent = null;
            curPos = -1;
            curCharPos = 0;
            separator = ',';
        }

        public static CsvData Read(string csv)
        {
            Reset();
            csvContent = csv;
            //行 列
            List<List<string>> dataTable = new List<List<string>>();
            List<string> recordData = new List<string>();
            dataTable.Add(recordData);

            string token = null;
            while ((token = GetToken()) != null)
            {
                //换行增加新纪录
                if (token == LFSTR)
                {
                    recordData = new List<string>();
                    dataTable.Add(recordData);
                }
                else
                {
                    recordData.Add(token);
                }
            }

            //移除所有尾部空行
            for (int i = dataTable.Count - 1; i >= 0; i--)
            {
                if (dataTable[i].Count == 0)
                {
                    dataTable.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            List<string> header = null;
            if (dataTable.Count > 1)
            {
                header = dataTable[0];
            }
            CsvData csvData = CsvData.Create(header);

            for (int i = 1; i < dataTable.Count; i++)
            {
                csvData.InsertData(dataTable[i].ToArray());
            }

            return csvData;
        }

        private static string GetToken()
        {
            if (Peek() == NUL)
            {
                return null;
            }
            if (Peek() == LF)
            {
                //遇到了尾部空行回车，直接算结尾
                if(Peek(2) == NUL)
                {
                    return null;
                }
                //正常换行
                GetChar();
                return LFSTR;
            }

            List<char> str = new List<char>(32);

            //双引号作用域
            bool isDoubleQm = false;
            //不等于分隔符或换行
            //第一个字符是双引号，进入字符串
            if (Peek() == '"')
            {
                isDoubleQm = true;
                GetChar();
            }
            while (true)
            {

                if (isDoubleQm)
                {
                    //双引号作用域
                    //意外的结束
                    if (Peek() == NUL)
                    {
                        throw new FormatException(string.Format("csv格式错误，行：{0} 位置：{1} 字符: {2}", curLine, curCharPos, curChar));
                    }

                    //判断转义或结尾
                    GetChar();

                    if (curChar == '"')
                    {
                        if (Peek() == '"')
                        {
                            //转义
                            str.Add('"');
                            GetChar();
                        }
                        else
                        {
                            //结束
                            if (Peek() == separator || Peek() == NUL)
                            {
                                GetChar();
                                return new string(str.ToArray());
                            }
                            else if (Peek() == LF)
                            {
                                return new string(str.ToArray());
                            }
                            else if (Peek() == CR && Peek(2) == LF)
                            {
                                //crlf  把cr吞了
                                GetChar();
                                return new string(str.ToArray());
                            }
                            else
                            {
                                //错误
                                throw new FormatException(string.Format("csv格式错误，行：{0} 位置：{1} 字符: {2}", curLine, curCharPos, curChar));
                            }
                        }
                    }
                    else
                    {
                        str.Add(curChar);
                    }

                }
                else
                {
                    if (Peek() == separator)
                    {
                        //把逗号吞了
                        GetChar();
                        return new string(str.ToArray());
                    }
                    else if (Peek() == LF)
                    {
                        return new string(str.ToArray());
                    }
                    else
                    {
                        str.Add(GetChar());
                    }
                }
            }

        }

    }
}
