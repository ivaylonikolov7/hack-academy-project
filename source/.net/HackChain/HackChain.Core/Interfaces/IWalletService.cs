using HackChain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Interfaces
{
    public interface IWalletService
    {
        void SetPrivateKey(string privateKeyHex);
        Task<Transaction> GenerateTransaction(string recipientAddress, long value, long fee);
    }
}
