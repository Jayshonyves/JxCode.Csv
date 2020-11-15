using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JxCode.CSV
{
    public class CsvConvert
    {
        public static IList<T> Deserialize<T>(string csvData) where T : new()
        {
            return CsvDataToObject<T>(CsvReader.Read(csvData));
        }
        public static string Serialize(object obj)
        {
            return CsvWriter.Write(ObjectToCsvData(obj));
        }

        public static IList<T> CsvDataToObject<T>(CsvData data) where T : new()
        {
            Type objType = typeof(T);
            List<T> list = new List<T>();
            //把csvData的Record导出到object
            foreach (CsvRecord record in data)
            {
                T tobj = new T();
                for (int i = 0; i < record.FieldCount; i++)
                {
                    SetValue(tobj, record.GetName(i), record[i]);
                }
                list.Add(tobj);
            }
            return list;
        }
        public static CsvData ObjectToCsvData(object obj)
        {
            CsvData csvData = CsvData.Create();

            Type t = obj.GetType();
            if (!typeof(IEnumerable).IsAssignableFrom(t))
            {
                throw new ArgumentException("对象没有实现IEnumerable");
            }

            //build header
            FieldInfo[] obj_fields = t.GetFields();
            foreach (var field in obj_fields)
            {
                csvData.Header.Add(field.Name);
            }

            var obj_props = t.GetProperties();
            foreach (PropertyInfo prop in obj_props)
            {
                csvData.Header.Add(prop.Name);
            }

            //build body
            IEnumerable enumerable = (IEnumerable)obj;
            IEnumerator record_it = enumerable.GetEnumerator();

            while (record_it.MoveNext())
            {
                object item = record_it.Current;
                if (item == null)
                {
                    continue;
                }
                //行内数据
                List<string> recordBuilder = new List<string>();
                Type itemType = item.GetType();
                FieldInfo[] fields = itemType.GetFields();
                foreach (FieldInfo field in fields)
                {
                    string recordItem = field.GetValue(item).ToString();
                    recordBuilder.Add(recordItem);
                }
                PropertyInfo[] propInfos = itemType.GetProperties();
                foreach (PropertyInfo prop in propInfos)
                {
                    string recordItem = prop.GetValue(item, null).ToString();
                    recordBuilder.Add(recordItem);
                }

                csvData.InsertData(recordBuilder.ToArray());
            }

            return csvData;
        }

        private static void SetValue(object obj, string name, object value)
        {
            Type type = obj.GetType();
            FieldInfo fieldInfo = type.GetField(name);
            if (fieldInfo != null)
            {
                //类型不一致
                if (fieldInfo.FieldType != value.GetType())
                {
                    value = Convert.ChangeType(value, fieldInfo.FieldType);
                }
                fieldInfo.SetValue(obj, value);
            }
            else
            {
                PropertyInfo propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType != value.GetType())
                    {
                        //类型不一致
                        value = Convert.ChangeType(value, propertyInfo.PropertyType);
                    }
                    propertyInfo.SetValue(obj, value, null);
                }
            }
        }
    }
}
