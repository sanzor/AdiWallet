using AdiWallet.Domain.Commands;
using LanguageExt;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Runner;

    public interface IRunner
    {
        EitherAsync<Error, CommandResult> RunAsync(Command command);
    }

