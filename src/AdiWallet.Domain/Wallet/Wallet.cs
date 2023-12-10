namespace AdiWallet.Domain.Wallet
{
    public class Wallet
    {
        public string Address { get; set; }
        public Dictionary<string,NFT> Nfts { get; set; }=new Dictionary<string, NFT>();
    }
}
