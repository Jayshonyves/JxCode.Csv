using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace JxCode.CSV
{
    public class CsvData : IEnumerable<CsvRecord>
    {
        public IList<string> Header { get; private set; }
        private IList<CsvRecord> data;

        private CsvData()
        {
        }

        public static CsvData Create(IList header = null)
        {
            CsvData self = new CsvData();
            if (header == null)
            {
                self.Header = new List<string>();
            }
            self.data = new List<CsvRecord>();
            return self;
        }

        public CsvRecord InsertData(params string[] data)
        {
            CsvRecord csvRecord = new CsvRecord(this, data);
            this.data.Add(csvRecord);
            return csvRecord;
        }

        public void RemoveData(CsvRecord record)
        {
            if (record != null && this.data.Contains(record))
            {
                this.data.Remove(record);
            }
        }
        public void RemoveData(string name)
        {
            CsvRecord record = null;
            foreach (CsvRecord item in this.data)
            {
                if (item.GetString(0) == name)
                {
                    record = item;
                    break;
                }
            }
            if (record != null)
            {
                this.data.Remove(record);
            }
        }


        public IEnumerator<CsvRecord> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }
    }

}
