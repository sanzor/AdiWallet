using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Messages
{
    public class Burn : Message
    {
        public const string NAME = "Burn";
        public override string Type => NAME;
    }
}
