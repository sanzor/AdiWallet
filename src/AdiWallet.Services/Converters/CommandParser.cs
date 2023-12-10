using AdiWallet.Domain.Commands;
using LanguageExt;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Converters
{
    internal class CommandParser:ICommandParser
    {
        public Either<Error, Command> ParseCommand(string[] commandLineArgs)

        {
            if (commandLineArgs.Length == 0)
            {
                return Error.New("Invalid input");
            }
            return Try(() =>
            {
                Command command = commandLineArgs[0].ToLowerInvariant() switch
                {
                    "--read-inline" => new ReadInline { Data = commandLineArgs[1] },
                    "--read-file" => new ReadFile { FilePath = commandLineArgs[1] },
                    "--reset" => new Reset { },
                    "--nft" => new NFTInfo { TokenID = commandLineArgs[1] },
                    "--wallet" => new WalletInfo { Address = commandLineArgs[1] },
                    _ => throw new NotSupportedException("invalid input")
                };
                return command;
            }).ToEither(Error.New);
           

        }
    }
}
