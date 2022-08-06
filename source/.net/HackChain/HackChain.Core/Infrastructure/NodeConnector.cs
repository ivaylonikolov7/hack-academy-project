using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Infrastructure
{
    public class NodeConnector : INodeConnector
    {
        private readonly HttpClient _httpClient;

        public NodeConnector(string baseUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
        }
        public Task AddBlock(Block block)
        {
            var response = Post<bool>("", block);
        }

        public async Task AddPeerNode(PeerNode peerNode)
        {
            var response = await Post<bool>("/api/node/peers", peerNode);
        }

        public async Task AddTransaction(Transaction transaction)
        {
            var response = await Post<bool>("/api/transactions/add", transaction);
        }

        public async Task<Block> GetBlockByIndex(long index)
        {
            var block = await Get<Block>("/api/block/index");

            return block;
        }

        public async Task<NodeStatus> GetNodeStatus()
        {
            var nodeStatus = await Get<NodeStatus>("/api/node/status");

            return nodeStatus;
        }

        public async Task<IEnumerable<PeerNode>> GetPeerNodes()
        {
            var nodes = await Get<IEnumerable<PeerNode>>("/api/node/peers");

            return nodes;
        }

        public Task GetPendingTransactions()
        {
            ApiResponse<TransactionDTO>
        }

        private async Task<T> Get<T>(string endpointUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpointUrl);
                if (response.IsSuccessStatusCode)
                {
                    //log? throw?
                }

                string content = await response.Content.ReadAsStringAsync();

                T result = JsonConvert.DeserializeObject<T>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<T> Post<T>(string endpointUrl, object payload)
        {
            try
            {
                var serializedPayload = JsonConvert.SerializeObject(payload);
                StringContent requestContent = new StringContent(serializedPayload, Encoding.UTF8);
                var response = await _httpClient.PostAsync(endpointUrl, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    //log? throw?
                }

                string content = await response.Content.ReadAsStringAsync();

                T result = JsonConvert.DeserializeObject<T>(content);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
