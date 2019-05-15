using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.PdfUtility
{
    public class PdfConverter : IPdfConverter
    {
        public async Task<FileContentResult> ConvertHtmlToPdfAsync(string content)
        {
            return await GetPdfConversion(content);
        }

        private async Task<FileContentResult> GetPdfConversion(string content)
        {
            //var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");
            var pdfHost = "http://weasyprint-develop-jhnamn.pathfinder.gov.bc.ca";
            string targetUrl = pdfHost + "/pdf";

            var client = new HttpClient();

            //var options = new PDF_Options
            //{
            //    Border = new PDF_Border
            //    {
            //        Top = ".25in",
            //        Right = ".25in",
            //        Bottom = ".25in",
            //        Left = ".25in"
            //    },
            //    Header = new PDF_Header
            //    {
            //        Height = "20mm",
            //        Contents = "<div><span style=\"float: left; font-size:14px\">Referral<br /></div><div style=\"float:right; vertical-align:top; text-align: right\"><span style=\"color: #444;\">Page {{page}}</span>/<span>{{pages}}</span><br />Printed: " + DateTime.Now.ToShortDateString() + "</div>"
            //    },
            //    Footer = new PDF_Footer
            //    {
            //        Height = "15mm",
            //        Contents = "<div></div><div style=\"float:right\">FOOTER</div>"
            //    },
            //    PaginationOffset = -1,
            //    Type = "pdf",
            //    Quality = "75",
            //    Format = "letter",
            //    Orientation = "portrait",
            //    Fontbase = "/usr/share/fonts/dejavu",
            //};

            try
            {
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
                return new FileContentResult(Encoding.ASCII.GetBytes($"Internal Server Error"), "text/plain");
            }
            catch (Exception e)
            {
                return new FileContentResult(Encoding.ASCII.GetBytes($"Internal Server Error: {e.Message}"), "text/plain");
            }
        }

        public class JSONResponse
        {
            public string type;
            public byte[] data;
        }

        public class PDFRequest
        {
            public string html;
            public string options;
        }

        public class PDF_Options
        {
            public string Type { get; set; }
            public string Quality { get; set; }
            public string Format { get; set; }
            public string Orientation { get; set; }
            public string Fontbase { get; set; }
            public PDF_Border Border { get; set; }
            public int PaginationOffset { get; set; }
            public PDF_Header Header { get; set; }
            public PDF_Footer Footer { get; set; }
        }

        public class PDF_Border
        {
            public string Top { get; set; }
            public string Right { get; set; }
            public string Bottom { get; set; }
            public string Left { get; set; }
        }

        public class PDF_Header
        {
            public string Height { get; set; }
            public string Contents { get; set; }
        }

        public class PDF_Footer
        {
            public string Height { get; set; }
            public string Contents { get; set; }
        }
    }
}
