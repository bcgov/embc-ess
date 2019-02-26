using Gov.Jag.Embc.Interfaces;
using Gov.Jag.Embc.Interfaces.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace DynamicsAutorest.Extensions
{
    public static class ClientExtensions
    {
        public static T GetEntityAttribute<T>(this DynamicsClient client, string cast, string entityName, string attributeName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var _baseUrl = client.BaseUri.AbsoluteUri;
            var _url = new System.Uri(new System.Uri(_baseUrl + (_baseUrl.EndsWith("/") ? "" : "/")), "EntityDefinitions").ToString();
            _url += $"(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/{cast}?";

            // Create HTTP transport objects
            var _httpRequest = new HttpRequestMessage();
            HttpResponseMessage _httpResponse = null;
            _httpRequest.Method = new HttpMethod("GET");
            _httpRequest.RequestUri = new System.Uri(_url);

            // Set Credentials
            if (client.Credentials != null)
            {
                client.Credentials.ProcessHttpRequestAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
            }
            _httpResponse = client.HttpClient.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            HttpStatusCode _statusCode = _httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string _responseContent = null;
            string _requestContent = null;
            if ((int)_statusCode != 200)
            {
                var ex = new OdataerrorException(string.Format("Operation returned an invalid status code '{0}'", _statusCode));
                try
                {
                    _responseContent = _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    Odataerror _errorBody = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<Odataerror>(_responseContent, client.DeserializationSettings);
                    if (_errorBody != null)
                    {
                        ex.Body = _errorBody;
                    }
                }
                catch (JsonException)
                {
                    // Ignore the exception
                }
                ex.Request = new HttpRequestMessageWrapper(_httpRequest, _requestContent);
                ex.Response = new HttpResponseMessageWrapper(_httpResponse, _responseContent);
                _httpRequest.Dispose();
                if (_httpResponse != null)
                {
                    _httpResponse.Dispose();
                }
                throw ex;
            }
            // Create Result
            var _result = new HttpOperationResponse<T>();
            _result.Request = _httpRequest;
            _result.Response = _httpResponse;
            // Deserialize Response
            if ((int)_statusCode == 200)
            {
                _responseContent = _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                try
                {
                    _result.Body = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<T>(_responseContent, client.DeserializationSettings);
                }
                catch (JsonException ex)
                {
                    _httpRequest.Dispose();
                    if (_httpResponse != null)
                    {
                        _httpResponse.Dispose();
                    }
                    throw new SerializationException("Unable to deserialize the response.", _responseContent, ex);
                }
            }
            return _result.Body;
        }
    }
}
