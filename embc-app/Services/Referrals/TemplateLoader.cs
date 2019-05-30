using System.IO;
using System.Reflection;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public class TemplateLoader
    {
        public static string LoadTemplate(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var manifestName = $"Gov.Jag.Embc.Public.Services.Referrals.Views.{name}.hbs";
            using (var stream = assembly.GetManifestResourceStream(manifestName))
            {
                using (var reader = new StreamReader(stream))
                {
                    string template = reader.ReadToEnd();
                    return template;
                }
            }
        }
    }
}
