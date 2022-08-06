using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Node.Web
{
    public class PeerNodeDTO
    {
        public string Id { get; set; }
        public string BaseUrl { get; set; }
        public int Rating { get; set; }
    }
}
