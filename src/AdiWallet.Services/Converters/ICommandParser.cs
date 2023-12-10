using AdiWallet.Domain.Commands;
using LanguageExt;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Converters;

    internal interface ICommandParser
    {
        Either<Error, Command> ParseCommand(string[] comandLineArgs);
    }

