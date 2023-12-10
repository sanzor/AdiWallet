using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Commands
{
    public class ReadInline : Command
    {
        public string Data { get; set; }
        public override CommandKind CommandKind => CommandKind.READ_INLINE;
    }
}
