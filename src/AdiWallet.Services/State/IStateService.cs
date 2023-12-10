using AdiWallet.Domain;
using AdiWallet.Domain.Messages;
using AdiWallet.Domain.Wallet;

namespace AdiWallet.Services.State
{
    internal interface IStateService
    {
        Either<Error, Wallet> GetWallet(string address);
        Either<Error, NFT> GetNFT(string tokenId);
        Either<Error, Unit> Reset();
        Either<Error, Unit> ApplyMessages(IEnumerable<Message> messages);

        Either<Error, AppState> GetNewestState();
    }
}
