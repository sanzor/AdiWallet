﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Commands
{
    public class NFTInfo : Command
    {
        public string TokenID { get; set; }
        public override CommandKind CommandKind => CommandKind.NFT_INFO;
    }
}
