using AdiWallet.Domain.Commands;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.CommandHandlers
{


    internal class WalletInfoHandler : CommandHandler, ICommandHandler

    {
        public WalletInfoHandler(IStateService stateService) : base(stateService)
        {
        }

        public CommandKind Kind => CommandKind.WALLET_INFO;

        public EitherAsync<Error, CommandResult> RunCommandAsync(Command command)
        {
            if (command.CommandKind is not CommandKind.WALLET_INFO)
            {
                return LeftAsync<Error, CommandResult>(Error.New("Invalid command"));
            }
            var walletInfoCommand = command as WalletInfo;
            var result =
                _stateService.GetWallet(walletInfoCommand.Address)
                .Bind(wallet =>
                {
                    if (wallet.Nfts.Count == 0)
                    {
                        Console.WriteLine($"Wallet {wallet.Address} holds no Tokens");
                    }
                    else
                    {
                        string message = $"Wallet {wallet.Address} holds {wallet.Nfts.Count} Tokens:";
                        foreach (var item in wallet.Nfts)
                        {
                            Console.WriteLine(item.Value.TokenId);
                        }
                    }
                   
                    return _stateService.GetNewestState();
                })
                .Map(state=>new CommandResult { CurrentState=state})
                .ToAsync();
            return result;
        }
    }
}
