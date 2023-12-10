using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Messages
{

    public class Mint : Message
    {
        public const string NAME = "Mint";
        
        public string Address { get; set; }
        public override string Type => NAME;
    }
}
