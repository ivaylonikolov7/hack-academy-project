using HackChain.Core.Infrastructure;
using Newtonsoft.Json;

namespace HackChain.Node.Web.Infrastructure
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        [JsonProperty("errors")]
        public List<ApiResponseError> Errors { get; set; } = new List<ApiResponseError>();
        public void AddError(string error, HackChainErrorCode errorCode = HackChainErrorCode.GenericError, string clientMessage = null)
        {
            Errors.Add(new ApiResponseError(error, errorCode, clientMessage));
        }

        [JsonProperty("data")]
        public T Data { get; set; }

        public static ApiResponse<T> Successful(T data)
        {
            return new ApiResponse<T>() { Data = data };
        }
    }
}
