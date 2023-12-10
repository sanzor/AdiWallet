
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdiWallet.Domain.Wallet;
namespace AdiWallet.Domain
{
    public class AppStateParams
    {
        public Dictionary<string,Wallet.Wallet> WalletMap { get; set; }
        public Dictionary<string,NFT> NFTMap { get; set; }
    }
}
