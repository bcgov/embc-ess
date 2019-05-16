using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Utils
{
    public class PdfConverter : IPdfConverter
    {
        public async Task<IActionResult> ConvertHtmlToPdfAsync(string content)
        {
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");
            string targetUrl = pdfHost + "/pdf";

            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, targetUrl)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            request.Headers.Clear();

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK) // success
            {
                var bytetask = response.Content.ReadAsByteArrayAsync();
                bytetask.Wait();

                return new FileContentResult(bytetask.Result, "application/pdf");
            }

            return new StatusCodeResult(500);
        }
    }
}
