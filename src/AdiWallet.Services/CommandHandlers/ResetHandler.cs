using AdiWallet.Domain.Commands;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.CommandHandlers
{
    internal class ResetHandler :CommandHandler, ICommandHandler
    {
        public ResetHandler(IStateService stateService) : base(stateService)
        {
        }

        public CommandKind Kind => CommandKind.RESET;

        public EitherAsync<Error, CommandResult> RunCommandAsync(Command command)
        {
            if(command.CommandKind is not CommandKind.RESET)
            {
                return Error.New("Invalid command");
            }
            var result=_stateService.Reset()
                
                .Bind(_=>_stateService.GetNewestState())
                .Map(state=>new CommandResult { CurrentState=state})
                .ToAsync();
            return result;
        }
    }
}
