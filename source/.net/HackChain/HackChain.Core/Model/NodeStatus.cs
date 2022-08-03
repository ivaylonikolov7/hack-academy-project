using System.Text.Json.Serialization;

namespace HackChain.Core.Model
{
    public class NodeStatus
    {
        public string NodeId { get; set; }
        public string BaseUrl { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NodeStatusType Status { get; set; }
        public bool IsMining { get; set; }
        public long LastBlockIndex { get; set; }
        public long LastBlockTimestamp { get; set; }
        public string LastBlockHash { get; set; }
        public long NonZeroAddressesCount { get; set; }
        public long ProcessedTransactionsCount { get; set; }
        public long PendingTransactionsCount { get; set; }
    }

    public enum NodeStatusType
    {
        Synced,
        Syncing
    }
}
