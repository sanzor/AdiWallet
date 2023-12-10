using AdiWallet.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services
{
    public interface ICommandProcessor
    {
        EitherAsync<Error,CommandResult> RunCommandAsync(Command command);
    }
}
