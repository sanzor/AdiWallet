using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Domain
{
    public class AdiWalletRunnerParams
    {
        public string StateFilePath { get; set; }
        public string[] CommandLineArgs { get; set; }
    }
}
