using AdiWallet.Domain;
using AdiWallet.Domain.Messages;
using AdiWallet.Domain.Wallet;
using LanguageExt.ClassInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.State
{
    internal class StateService : IStateService
    {

        public StateService(AppState state)
        {
          _appState= _oldState =state ?? throw new ArgumentNullException(nameof(state));
        
        }
        private  AppState _appState;
        private readonly AppState _oldState;
        
      

        public Either<Error, Unit> ApplyMessages(IEnumerable<Message> messages)
        {
          
            foreach (var item in messages)
            {
                if (!ApplyMessage(item))
                {
                    _appState = _oldState;
                    return Error.New("Invalid message batch");
                }
                continue;
            }
           
            return Unit.Default;
        }
        private bool ApplyMessage(Message message)
        {
            var result = (message.Type) switch
            {
                Mint.NAME => HandleMint(message as Mint),
                Burn.NAME => HandleBurn(message as Burn),
                Transfer.NAME => HandleTransfer(message as Transfer)
            };
            return result;
        }

        private bool HandleMint(Mint mintMessage)
        {
            if(_appState.NftMap.TryGetValue(mintMessage.TokenId,out var _))
            {
                return false;
            }
            if (!_appState.WalletMap.TryGetValue(mintMessage.Address, out var _))
            {
                var newWallet = new Wallet { Address = mintMessage.Address };
                _appState.WalletMap.Add(newWallet.Address, newWallet);
            }
            var nft = new NFT { Address = mintMessage.Address, TokenId = mintMessage.TokenId };
            _appState.WalletMap[mintMessage.Address].Nfts.Add(nft.TokenId, nft);
            _appState.NftMap.Add(mintMessage.TokenId, nft);
            return true;

        }

        private bool HandleBurn(Burn burnMessage)
        {
            if (!_appState.NftMap.TryGetValue(burnMessage.TokenId, out var token))
            {
                return false;
            }
            if (!_appState.WalletMap.TryGetValue(token.Address, out var _))
            {
                return false;
            }
            return _appState.WalletMap.Remove(token.Address) && _appState.NftMap.Remove(token.TokenId);
        }

        private bool HandleTransfer(Transfer transferMessage)
        {
            if (!_appState.NftMap.TryGetValue(transferMessage.TokenId, out var token))
            {
                return false;
            }
            if (!_appState.WalletMap.TryGetValue(transferMessage.From, out var fromWallet))
            {
                return false;
            }
            if (!_appState.WalletMap.TryGetValue(transferMessage.To, out var toWallet))
            {
                
                var newToWallet = new Wallet { Address = transferMessage.To };
                token.Address = newToWallet.Address;
                newToWallet.Nfts.Add(token.TokenId, token);
                _appState.WalletMap.Add(transferMessage.To, newToWallet);

                if (!fromWallet.Nfts.Remove(token.TokenId))
                {
                    return false;
                }
                
                return true;

            }
            token.Address = toWallet.Address;
            if (!fromWallet.Nfts.Remove(token.TokenId))
            {
                return false;
            }
            toWallet.Nfts.Add(token.TokenId,token);
            token.Address = toWallet.Address;
            return true;
        }

        public Either<Error, Unit> Reset()
        {
            _appState = _oldState;
            return Unit.Default;
            
        }

        public Either<Error, NFT> GetNFT(string tokenId)
        {
            return
                _appState.NftMap.TryGetValue(tokenId, out var nft) ?
                nft :
                Error.New($"Nft: {tokenId} does not exist");
        }

        public Either<Error, Wallet> GetWallet(string address)
        {
            return
                _appState.WalletMap.TryGetValue(address, out Wallet wallet) ?
                wallet :
                Error.New($"Wallet: {address} does not exist");
        }
        
        public Either<Error,AppState> GetNewestState()
        {
            if(_appState==null || _oldState == null)
            {
                return Left<Error, AppState>("Invalid state");
            }
            return this._appState;
        }

       
        
    }
}
