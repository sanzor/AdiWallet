using AdiWallet.Domain.Messages;
using AdiWallet.Domain.Wallet;
using LanguageExt;
using LanguageExt.Common;
using System.Text.Json.Serialization;

namespace AdiWallet.Domain
{
    public class AppState
    {
        public AppState()
        {

        }
        //public AppState(AppStateParams @params)
        //{
        //    WalletMap = @params.WalletMap??throw new ArgumentNullException(nameof(@params.WalletMap));
        //    NftMap = @params.NFTMap??throw new ArgumentNullException(nameof(@params.NFTMap));
        //}
        /// <summary>
        /// WalletMap
        /// </summary>
      
        public Dictionary<string, Wallet.Wallet> WalletMap { get; set; }= new Dictionary<string, Wallet.Wallet>();
       
       
        public Dictionary<string, NFT> NftMap { get; set; }= new Dictionary<string, NFT>();

     
       
       
       

    }
}
