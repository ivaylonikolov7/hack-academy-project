using HackChain.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace HackChain.Node.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HackChainExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var hackChainExpcetion = context.Exception as HackChainException;
            HttpResponseMessage response;
            HttpStatusCode httpCode;
            HackChainErrorCode errorCode;
            string errorMessage;
            string clientMessage = string.Empty;

            if (hackChainExpcetion != null)
            {
                errorMessage = hackChainExpcetion.Message;
                httpCode = MapRpErrorToHttpError(hackChainExpcetion.ErrorCode);
                errorCode = hackChainExpcetion.ErrorCode;
                clientMessage = hackChainExpcetion.ClientMessage;
            }
            else
            {
                errorMessage = context.Exception.ToString();
                httpCode = HttpStatusCode.InternalServerError;
                errorCode = HackChainErrorCode.GenericError;
            }

            var apiResponse = new ApiResponse<string> { Data = null };
            apiResponse.AddError(errorMessage, errorCode, clientMessage);
            string content = JsonConvert.SerializeObject(apiResponse);

            context.HttpContext.Response.StatusCode = (int)httpCode;
            context.Result = new JsonResult(apiResponse);
        }

        private HttpStatusCode MapRpErrorToHttpError(HackChainErrorCode errorCode)
        {
            HttpStatusCode statusCode;
            switch (errorCode)
            {
                case HackChainErrorCode.GenericError:

                case HackChainErrorCode.Transaction_Invalid_Sender:
                case HackChainErrorCode.Transaction_Invalid_Recipient:
                case HackChainErrorCode.Transaction_Invalid_Value:
                case HackChainErrorCode.Transaction_Invalid_Fee:
                case HackChainErrorCode.Transaction_Invalid_Hash:
                case HackChainErrorCode.Transaction_Invalid_Signature:
                case HackChainErrorCode.Transaction_Duplicate:
                    statusCode = HttpStatusCode.BadRequest;
                    break;


                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            return statusCode;
        }
    }
}
