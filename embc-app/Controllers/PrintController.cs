using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace embc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : Controller
    {
        [HttpPost]
        public async Task<FileContentResult> Create([FromBody] object item)
        {
            return new FileContentResult(null, "application/pdf");
        }

        private async Task<FileContentResult> PrintReportAsync(string content, bool portrait)
        {
            string reportHeader = string.Empty;

            FileContentResult result = null;
            var pdfHost = Environment.GetEnvironmentVariable("PDF_SERVICE_NAME");

            string targetUrl = pdfHost + "/api/PDF/BuildPDF";

            PDF_Options options = new PDF_Options();
            options.Border = new PDF_Border();
            options.header = new PDF_Header();
            options.Footer = new PDF_Footer();
            options.paginationOffset = -1;

            options.Type = "pdf";
            options.Quality = "75";
            options.Format = "letter";
            options.Orientation = (portrait) ? "portrait" : "landscape";
            options.fontbase = "/usr/share/fonts/dejavu";
            options.Border.Top = ".25in";
            options.Border.Right = ".25in";
            options.Border.Bottom = ".25in";
            options.Border.Left = ".25in";
            options.header.height = "20mm";
            options.header.contents = "<div><span style=\"float: left; font-size:14px\">Farm Name: " + _ud.FarmDetails().farmName + "<br />" +
                                      "Planning Year: " + _ud.FarmDetails().year + "</span></div><div style=\"float:right; vertical-align:top; text-align: right\"><span style=\"color: #444;\">Page {{page}}</span>/<span>{{pages}}</span><br />Printed: " + DateTime.Now.ToShortDateString() + "</div>";
            options.Footer.Height = "15mm";
            options.Footer.Contents = "<div></div><div style=\"float:right\">Version " + _sd.GetStaticDataVersion() + "</div>";

            // call the microservice
            try
            {
                PDFRequest req = new PDFRequest();

                HttpClient client = new HttpClient();

                reportHeader = await RenderHeader();

                string rawdata = "<!DOCTYPE html>" +
                    "<html>" +
                    reportHeader +
                    "<body>" +
                    //"<div style='display: table; width: 100%'>" +
                    //"<div style='display: table-row-group; width: 100%'>" +
                    content +
                    //"</div>" +
                    //"</div>" +
                    "</body></html>";

                req.html = rawdata;
                req.options = JsonConvert.SerializeObject(options);
                req.options = req.options.Replace("fontbase", "base");

                //FileContentResult res = await BuildPDF(nodeServices, req);

                //return res;

                string payload = JsonConvert.SerializeObject(req);

                var request = new HttpRequestMessage(HttpMethod.Post, targetUrl);
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                request.Headers.Clear();
                // transfer over the request headers.
                foreach (var item in Request.Headers)
                {
                    string key = item.Key;
                    string value = item.Value;
                    request.Headers.Add(key, value);
                }

                Task<HttpResponseMessage> responseTask = client.SendAsync(request);
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;

                if (response.StatusCode == HttpStatusCode.OK) // success
                {
                    var bytetask = response.Content.ReadAsByteArrayAsync();
                    bytetask.Wait();

                    result = new FileContentResult(bytetask.Result, "application/pdf");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            return result;
        }

        public class PDF_Options
        {
            public string Type { get; set; }
            public string Quality { get; set; }
            public string Format { get; set; }
            public string Orientation { get; set; }
            public string Fontbase { get; set; }
            public PDF_Border Border { get; set; }
            public int paginationOffset { get; set; }
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
