using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Gov.Jag.Embc.Public.Utils
{
    //inspired by https://stackoverflow.com/questions/1890093/converting-a-generic-list-to-a-csv-string
    public static class CsvConverter
    {
        private static void CreateHeader<T>(IEnumerable<T> list, TextWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(IEnumerable<T> list, TextWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }

        public static void CreateCSV<T>(this IEnumerable<T> list, string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                CreateHeader(list, sw);
                CreateRows(list, sw);
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> list)
        {
            using (var sw = new StringWriter())
            {
                CreateHeader(list, sw);
                CreateRows(list, sw);
                return sw.ToString();
            }
        }
    }
}
