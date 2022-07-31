using HackChain.Core.Model;
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
            foreach (var tr in transactions)
            {
                block.Data.Add(tr);
            }
        }

        public static void Validate(this Block block)
        {
            // 

            
        }
    }
}
