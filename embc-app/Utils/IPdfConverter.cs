using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.Utils
{
    public interface IPdfConverter
    {
        Task<byte[]> ConvertHtmlToPdfAsync(string content);
    }
}
