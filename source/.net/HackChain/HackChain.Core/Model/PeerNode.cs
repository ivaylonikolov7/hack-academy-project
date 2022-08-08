using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Model
{
    public class PeerNode
    {
        public string Id { get; set; }
        public string BaseUrl { get; set; }
        public int Rating { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

    }
}
