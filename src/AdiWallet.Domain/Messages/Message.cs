using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Messages
{
    public abstract class Message
    {

        public string TokenId { get; set; }
        public abstract string Type { get; }
    }
}
