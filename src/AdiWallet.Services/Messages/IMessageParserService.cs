using AdiWallet.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Messages
{
    internal interface IMessageParserService
    {
        Either<Error, List<Message>> ParseMessages(string data);
    }
}
