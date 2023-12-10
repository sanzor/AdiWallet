using AdiWallet.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.CommandHandlers
{
    public interface ICommandHandler
    {
        public  CommandKind Kind { get; }
        public  EitherAsync<Error, CommandResult> RunCommandAsync(Command command);
    }
}
