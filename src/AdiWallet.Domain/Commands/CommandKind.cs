using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Commands
{
    public enum CommandKind
    {
        READ_INLINE=0,
        READ_FILE=1,
        NFT_INFO=2,
        WALLET_INFO=3,
        RESET=4
    }
}
