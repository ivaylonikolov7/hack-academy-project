using HackChain.Core.Infrastructure;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HackChain.Node.Web.Infrastructure
{
    public class ApiResponseError
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonProperty("rpErrorCode")]
        public HackChainErrorCode ErrorCode { get; set; }

        [JsonProperty("clientMessage")]
        public string ClientMessage { get; set; }

        public ApiResponseError(string error, HackChainErrorCode errorCode, string clientMessage = null)
        {
            Error = error;
            ErrorCode = errorCode;
            ClientMessage = clientMessage;
        }
    }
}
