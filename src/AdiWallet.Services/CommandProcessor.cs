using AdiWallet.Domain.Commands;
using AdiWallet.Services.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<CommandKind, ICommandHandler> _commandHandlersMap;
        public EitherAsync<Error,CommandResult> RunCommandAsync(Command command)
        {
            if(!_commandHandlersMap.TryGetValue(command.CommandKind,out var handler))
            {
                return Error.New(new NotSupportedException("Command not supported"));
            }
            var result = handler.RunCommandAsync(command);
            return result;
        }
        public CommandProcessor(Dictionary<CommandKind,ICommandHandler> commandHandlersMap)
        {
            _commandHandlersMap=commandHandlersMap??throw new ArgumentNullException(nameof(commandHandlersMap));
        }
    }
}
