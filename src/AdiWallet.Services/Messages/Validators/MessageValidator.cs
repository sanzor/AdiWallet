using AdiWallet.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Messages.Validators
{
    internal class MessageValidator:IMessageValidator
    {
        public bool ValidateMessage(Message message)
        {
            var result=message.Type switch
            {
                Burn.NAME => ValidateMessage(message as Burn),
                Transfer.NAME => ValidateMessage(message as Transfer),
                Mint.NAME => ValidateMessage(message as Mint)
            };
            return result;
        }
        private bool ValidateMessage(Burn message)
        {
            if (message.TokenId == null)
            {
                return false;
            }
            return true;
        }
        private bool ValidateMessage(Mint message)
        {
            if(message.TokenId is null || message.Address is null)
            {
                return false;
            }
            return true;
        }
        private bool ValidateMessage(Transfer message)
        {
            if (message.TokenId is null || message.From is null || message.To is null)
            {
                return false;
            }
            return true;
        }
    }
}
