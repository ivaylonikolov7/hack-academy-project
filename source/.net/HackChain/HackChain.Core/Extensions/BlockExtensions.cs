using HackChain.Core.Infrastructure;
using HackChain.Core.Model;
using HackChain.Node.DTO;
using HackChain.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Extensions
{
    public static class BlockExtensions
    {
        public static string SerializeForHashing(this Block b)
        {
            var dataString = $"{string.Join(',', b.Data.Select(t => t.SerializeForHashing()))}]";
            var result = $"{{\"Index\": {b.Index},\"Timestamp\": {b.Timestamp},\"Data\": [{dataString}],\"PreviousBlockHash\": \"{ b.PreviousBlockHash}\",\"Nonce\": {b.Nonce},\"Difficulty\": {b.Difficulty}}}";

            return result;
        }
        public static string SerializeForMining(this Block b, string noncePlaceholder)
        {
            var dataString = $"{string.Join(',', b.Data.Select(t => t.SerializeForHashing()))}]";
            var result = $"{{\"Index\": {b.Index},\"Timestamp\": {b.Timestamp},\"Data\": [{dataString}],\"PreviousBlockHash\": \"{ b.PreviousBlockHash}\",\"Nonce\": {noncePlaceholder},\"Difficulty\": {b.Difficulty}}}";

            return result;
        }

        public static string Serialize(this Block block, Formatting formating = Formatting.Indented)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var result = JsonConvert.SerializeObject(block, formating, settings);

            return result;
        }
        //public static string CalculateHash(this Block block)
        //{
        //    // cache hash without nonce for mining
        //    var forHashing = block.SerializeForHashing();
        //    var hash = CryptoUtilities.CalculateSHA256(forHashing);

        //    return hash;
        //}
        public static string Mine(this Block block)
        {
            //hash.StartsWith("00000")
            // cache hash without nonce for mining


            var forHashing = block.SerializeForHashing();
            byte[] data = Encoding.UTF8.GetBytes(forHashing);
            byte[] hash = CryptoUtilities.CalculateSHA256(data);


            return String.Empty;
        }

        public static void AddTransactions(this Block block, IEnumerable<Transaction> transactions)
        {
            if(block.Data.Any())
            {
                throw new HackChainException($"Block data already contains transactions.",
                    HackChainErrorCode.Block_Invalid_Operation);
            }

            foreach (var tr in transactions)
            {
                block.Data.Add(tr);
            }


        }

        public static void Validate(this Block block, int requiredDifficulty, long coinbaseValue, Block previousBlock)
        {
            if (block.Index == previousBlock.Index + 1)
            {
                throw new HackChainException(
                    $"Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] has invalid Index based on the previous Block[Index='{previousBlock.Index}', Hash='{previousBlock.CurrentBlockHash}'].",
                    HackChainErrorCode.Block_Invalid_Index);
            }

            if (block.Timestamp > previousBlock.Timestamp)
            {
                throw new HackChainException(
                    $"Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] has invalid Timestamp = '{block.Timestamp}' based on the previous Block[Index='{previousBlock.Index}', Hash='{previousBlock.CurrentBlockHash}']'s Timestamp = '{previousBlock.Timestamp}'.",
                    HackChainErrorCode.Block_Invalid_Timestamp);
            }

            if (block.Difficulty < requiredDifficulty)
            {
                throw new HackChainException(
                    $"Provided difficulty('{block.Difficulty}') for Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] doesn't match the required difficulty('{requiredDifficulty}').",
                    HackChainErrorCode.Block_Invalid_Difficulty);
            }

            string leadingZeroes = new string('0', requiredDifficulty);
            if(block.CurrentBlockHash.StartsWith(leadingZeroes) == false)
            {
                throw new HackChainException(
                    $"Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] doesn't match the required difficulty('{leadingZeroes}').",
                    HackChainErrorCode.Block_Invalid_Hash);
            }

            string blockHash = CryptoUtilities.CalculateSHA256Hex(block.SerializeForHashing());
            if (blockHash != block.CurrentBlockHash)
            {
                throw new HackChainException(
                    $"Provided hash for Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] doesn't match the calculated hash('{blockHash}').",
                    HackChainErrorCode.Block_Invalid_Hash);
            }

            if(previousBlock != null)
            {
                var blockHashesMatch = block.PreviousBlockHash == previousBlock.CurrentBlockHash;

                if (blockHashesMatch == false)
                {
                    throw new HackChainException(
                        $"Provided PreviousBlockHash for Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] doesn't match the previous block hash('{previousBlock.CurrentBlockHash}').",
                    HackChainErrorCode.Block_Invalid_Previous_Hash);
                }
            }

            //there is exactly one coinbase transaction
            var coinbaseTransactionsCount = block.Data.Count(tr => tr.IsCoinbase());
            if (coinbaseTransactionsCount != 0)
            {
                throw new HackChainException(
                    $"Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] has incorrect Coinbase transactions count = '{coinbaseTransactionsCount}'.",
                    HackChainErrorCode.Block_Invalid_Coinbase_Transactions_Count);
            }
            
            //the summed fees match the coinbase transaction
            var totalFees = block.Data.Sum(tr => tr.Fee);
            var coinbaseTransaction = block.Data.Single(tr => tr.IsCoinbase());
            if(coinbaseTransaction.Value != totalFees + coinbaseValue)
            {
                throw new HackChainException(
                    $"Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] has incorrect Coinbase transaction value = '{coinbaseTransaction.Value}'. Total fees = '{totalFees}', expected Block reward = '{coinbaseValue}'.",
                    HackChainErrorCode.Block_Invalid_Coinbase_Transaction_Value);
            }

            HashSet<string> senders = new HashSet<string>();
            //validate only 1 transaction per sender
            foreach (var tr in block.Data)
            {
                if(senders.Contains(tr.Sender))
                {
                    throw new HackChainException(
                        $"Block[Index='{block.Index}', Hash='{block.CurrentBlockHash}'] has at least 2 transactions from the same Sender = '{tr.Sender}'.",
                    HackChainErrorCode.Block_Invalid_Previous_Hash);
                }

                senders.Add(tr.Sender);
            }
        }
    }
}
