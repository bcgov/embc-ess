namespace Gov.Jag.Embc.Public.Utils
{
    class JsonDateConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter
    {
        public JsonDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
