using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain.Commands
{
    public class ReadFile : Command
    {
        public string FilePath { get; set; }
        public override CommandKind CommandKind => CommandKind.READ_FILE;
    }
}
