using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.PdfUtility
{
    public interface IPdfConverter
    {
        Task<IActionResult> ConvertHtmlToPdfAsync(string content);
    }
}
