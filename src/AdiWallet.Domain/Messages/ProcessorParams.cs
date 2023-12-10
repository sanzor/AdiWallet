using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Messages
{
    public class ProcessorParams
    {
        IEnumerable<Message> InputMessages { get; set; }
    }
}
