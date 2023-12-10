using AdiWallet.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Messages.Validators
{
    internal interface IMessageValidator
    {
        bool ValidateMessage(Message message);
    }
}
