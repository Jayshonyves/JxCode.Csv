using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JxCode.CSV
{
    public class CsvRecord : IDataRecord, IEnumerable<string>
    {
        private CsvData csvData;
        private IList<string> data;

        public CsvRecord(CsvData csvData, IEnumerable<string> data)
        {
            this.csvData = csvData;
            this.data = new List<string>(data);
        }

        public object this[int i] => this.data[i];


        public object this[string name]
        {
            get
            {
                int pos = this.csvData.Header.IndexOf(name);
                if(pos == -1)
                {
                    return null;
                }
                return this.data[pos];
            }
        }

        public int FieldCount => this.data.Count;

        public bool GetBoolean(int i)
        {
            return bool.Parse(this.data[i]);
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return char.Parse(this.data[i]);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return DateTime.Parse(this.data[i]);
        }

        public decimal GetDecimal(int i)
        {
            return decimal.Parse(this.data[i]);
        }

        public double GetDouble(int i)
        {
            return double.Parse(this.data[i]);
        }

        public Type GetFieldType(int i)
        {
            return typeof(string);
        }

        public float GetFloat(int i)
        {
            return float.Parse(this.data[i]);
        }

        public Guid GetGuid(int i)
        {
            return Guid.Parse(this.data[i]);
        }

        public short GetInt16(int i)
        {
            return short.Parse(this.data[i]);
        }

        public int GetInt32(int i)
        {
            return int.Parse(this.data[i]);
        }

        public long GetInt64(int i)
        {
            return long.Parse( this.data[i]);
        }

        public string GetName(int i)
        {
            return this.csvData.Header[i];
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return this.data[i];
        }

        public object GetValue(int i)
        {
            return this.data[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
    }
}
