using Gov.Jag.Embc.Public.ViewModels.Search;
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

        /// <summary>
        /// Creates a CSV with a list of search parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="searchParams">The search parameters to include in the CSV</param>
        /// <param name="isEvacueeExport">Flag to control which report it is</param>
        /// <returns></returns>
        public static string ToCSV<T>(this IEnumerable<T> list, EvacueeSearchQueryParameters searchParams, bool isEvacueeExport)
        {
            using (var sw = new StringWriter())
            {
                AddSearchParams(sw, searchParams, isEvacueeExport);
                CreateHeader(list, sw);
                CreateRows(list, sw);
                return sw.ToString();
            }
        }

        private static void AddSearchParams(StringWriter sw, EvacueeSearchQueryParameters searchParams, bool isEvacueeExport)
        {
            PropertyInfo[] properties = typeof(EvacueeSearchQueryParameters).GetProperties();

            // Add Search header
            sw.Write("Search Parameters");
            sw.Write(sw.NewLine);

            for (int i = 0; i < properties.Length - 1; i++)
            {
                object prop = properties[i].GetValue(searchParams);
                string propName = GetFriendlySearchParamName(properties[i].Name);
                ProcessProperty(prop, propName, sw, isEvacueeExport);
                if (IsValidProp(prop, propName))
                {
                    sw.Write(propName + ":" + ",");
                    sw.Write(properties[i].GetValue(searchParams).ToString());
                    sw.Write(sw.NewLine);
                }
            }
            sw.Write(sw.NewLine);
        }

        private static bool IsValidProp(object prop, string propName)
        {
            // Ensure property is not null
            bool result = prop != null && !string.IsNullOrEmpty(prop.ToString());
            // Ensure property is not one we ignore (limit, offset, sortby, etc.)
            if (result)
            {
                    switch (propName.ToLower())
                {
                    case "offset":
                        result = false;
                        break;
                    case "limit":
                        result = false;
                        break;
                    case "sortby":
                        result = false;
                        break;
                    case "referrals provided":
                        result = false;
                        break;
                    case "reg completed":
                        result = false;
                        break;
                    default:
                        result = true;
                        break;
                }
            }
            return result;
        }


        private static void ProcessProperty(object prop, string propName, StringWriter sw, bool isEvacueeExport)
        {
            // These radio button properties have hard coded values per report
            bool isRadioBtn = false;
            switch (propName)
            {
                case "Referrals Provided":
                    isRadioBtn = true;
                    break;
                case "Reg Completed":
                    isRadioBtn = true;
                    break;
                default:
                    break;
            }

            if (isRadioBtn)
            {
                if (isEvacueeExport)
                {
                    sw.Write($"{propName}:, Show all");
                }
                else
                {
                    sw.Write($"{propName}:, Yes");
                }
                
                sw.Write(sw.NewLine);
            }

        }

        // Returns a user friendly name for the search parameters 
        private static string GetFriendlySearchParamName(string propName)
        {
            string result = string.Empty;

            switch (propName.ToLower())
            {
                case "lastname":
                    result = "Last Name";
                    break;
                case "firstname":
                    result = "First Name";
                    break;
                case "incidenttasknumber":
                    result = "Task #";
                    break;
                case "essfilenumber":
                    result = "ESS File #";
                    break;
                case "evacuatedfrom":
                    result = "Evacuated To";
                    break;
                case "evacuatedto":
                    result = "Evacuated From";
                    break;
                case "hasreferrals":
                    result = "Referrals Provided";
                    break;
                case "registrationcompleted":
                    result = "Reg Completed";
                    break;
                case "dateofbirth":
                    result = "Date of Birth";
                    break;
                default: 
                    result = propName;
                    break;
            }

            return result;
        }
           
    }
}
    

