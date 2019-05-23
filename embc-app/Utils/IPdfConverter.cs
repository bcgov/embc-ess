using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public interface IPdfConverter
    {
        Task<byte[]> ConvertHtmlToPdfAsync(string content);
    }
}
