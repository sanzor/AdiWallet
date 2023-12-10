using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Messages
{
    public class Transfer : Message
    {
        public const string NAME = "Transfer";
        public override string Type => Transfer.NAME;
        public string From { get; set; }
        public string To { get; set; }
       
    }
}
