using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public class PdfConverter : IPdfConverter
    {
        public async Task<byte[]> ConvertHtmlToPdfAsync(string content)
        {
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");
            var fileName = $"referral_{DateTime.Now.ToString("ddMMMMyyyy")}_{DateTime.Now.ToString("HHmmtt")}.pdf";
            string targetUrl = $"{pdfHost}/pdf?filename={fileName}";

            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, targetUrl)
            {
                Content = new StringContent(content, Encoding.UTF8, "text/html")
            };

            request.Headers.Clear();

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            throw new Exception($"Failed to generate PDF: {response.StatusCode}, {response.ReasonPhrase}");
        }
    }
}
