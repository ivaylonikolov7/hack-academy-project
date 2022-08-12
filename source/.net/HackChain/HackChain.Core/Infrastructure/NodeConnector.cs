using HackChain.Core.Interfaces;
using HackChain.Node.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

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

        public async Task<bool> AddBlock(BlockDTO block)
        {
            var response = await Post<bool>("", block);

            return response;
        }

        public async Task<bool> AddPeerNode(PeerNodeDTO peerNode)
        {
            var response = await Post<bool>("/api/node/peers", peerNode);
            return response;
        }

        public async Task<bool> AddTransaction(TransactionDTO transaction)
        {
            var response = await Post<bool>("/api/transactions/add", transaction);

            return response;
        }

        public async Task<BlockDTO> GetBlockByIndex(long index)
        {
            var block = await Get<BlockDTO>($"/api/blocks/{index}");

            return block;
        }

        public async Task<NodeStatusDTO> GetNodeStatus()
        {
            var nodeStatus = await Get<NodeStatusDTO>("/api/node/status");

            return nodeStatus;
        }

        public async Task<IEnumerable<PeerNodeDTO>> GetPeerNodes()
        {
            var nodes = await Get<IEnumerable<PeerNodeDTO>>("/api/node/peers");

            return nodes;
        }

        public async Task<IEnumerable<TransactionDTO>> GetPendingTransactions()
        {
            var pendingTransactions = await Get<IEnumerable<TransactionDTO>>("/api/transactions/pending");

            return pendingTransactions;
        }

        private async Task<T> Get<T>(string endpointUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpointUrl);
                var result = await ProcessApiResponse<T>(response);

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
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(endpointUrl, requestContent);
                var result = await ProcessApiResponse<T>(response);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<T> ProcessApiResponse<T>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode == false)
            {
                return default;
            }

            string content = await responseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ApiResponse<T>>(content);

            // check for null result
            if (response == null)
            {
                // log errors and throw?
            }
            if (response.Success == false)
            {
                // log errors? throw?
            }

            return response.Data;
        }
    }
}
