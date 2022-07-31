using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Infrastructure
{
    public class HackChainException : Exception
    {
        public HackChainErrorCode ErrorCode { get; private set; }
        public string ClientMessage { get; set; }

        public HackChainException(
            string message,
            HackChainErrorCode errorCode,
            string clientMessage = null,
            Exception innerException = null)

            : base(message, innerException)
        {
            ErrorCode = errorCode;
            ClientMessage = clientMessage;
        }
    }
}
