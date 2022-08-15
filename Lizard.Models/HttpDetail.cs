using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class HttpDetail
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; } = string.Empty;

        [JsonPropertyName("method")]
        public string Method { get; set; } = "GET";

        [JsonPropertyName("requestBody")]
        public string RequestBody { get; set; } = string.Empty;

        [JsonPropertyName("requestHeaders")]
        public string RequestHeaders { get; set; } = string.Empty;


        [JsonPropertyName("responseHeaders")]
        public string ResponseHeaders { get; set; } = string.Empty;

        [JsonPropertyName("responseBody")]
        public string ResponseBody { get; set; } = string.Empty;
    }
}
