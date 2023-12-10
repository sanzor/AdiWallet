using AdiWallet.Domain;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Runner
{
    internal interface IStateProvider
    {
        EitherAsync<Error, AppState> LoadStateAsync();
        EitherAsync<Error, Unit> WriteStateAsync(AppState state);
    }
}
