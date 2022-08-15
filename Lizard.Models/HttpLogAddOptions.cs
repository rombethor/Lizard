using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class HttpLogAddOptions
    {
        [JsonPropertyName("endpoint"), Required, MaxLength(255)]
        public string Uri { get; set; } = string.Empty;

        [JsonPropertyName("method"), Required, MaxLength(10)]
        public string Method { get; set; } = string.Empty;

        [JsonPropertyName("requestBody"), MaxLength(5000)]
        public string? RequestBody { get; set; }

        [JsonPropertyName("status"), Range(100, 599)]
        public int? StatusResult { get; set; }

        [JsonPropertyName("responseBody"), MaxLength(5000)]
        public string? ResponseBody { get; set; }

        [JsonPropertyName("requestHeaders"), MaxLength(512)]
        public string? RequestHeaders { get; set; }

        [JsonPropertyName("responseHeaders"), MaxLength(512)]
        public string? ResponseHeaders { get; set; }
    }
}
