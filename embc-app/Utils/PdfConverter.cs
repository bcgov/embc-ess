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
            var fileName = $"?filename=referral_{DateTime.Now.ToString("ddMMMMyyyy")}_{DateTime.Now.ToString("HHmmtt")}.pdf";
            string targetUrl = $"{pdfHost}/pdf{fileName}";

            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, targetUrl)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            request.Headers.Clear();

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var bytetask = response.Content.ReadAsByteArrayAsync();
                bytetask.Wait();

                return bytetask.Result;
                //return new FileContentResult(bytetask.Result, "application/pdf");
            }

            return null;
        }
    }
}
