using AdiWallet.Domain.Commands;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.CommandHandlers
{
    internal class NFTInfoHandler : CommandHandler, ICommandHandler
    {
        public NFTInfoHandler(IStateService stateService) : base(stateService)
        {
        }

        public CommandKind Kind => CommandKind.NFT_INFO;

        public EitherAsync<Error, CommandResult> RunCommandAsync(Command command)
        {
            if (command.CommandKind is not CommandKind.NFT_INFO)
            {
                return LeftAsync<Error, CommandResult>(Error.New("Invalid command"));
            }
            var nftInfoCommand = command as NFTInfo;
            var result = 
                _stateService.GetNFT(nftInfoCommand.TokenID)
                .Bind(nft =>
                {
                    var message = nft.Address is null ?
                        $"Token {nft.TokenId} is not owned by any wallet" :
                        $"Token {nft.TokenId} is owned by {nft.Address}";
                    Console.WriteLine(message);
                    return _stateService.GetNewestState();
                })
                .Map(state=>new CommandResult { CurrentState=state})
                .ToAsync();
            return result;

        }
    }
}
