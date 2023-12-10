using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Commands
{
    public class WalletInfo : Command
    {
        public string Address { get; set; }
        public override CommandKind CommandKind => CommandKind.WALLET_INFO;
    }
}
