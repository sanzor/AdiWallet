using AdiWallet.Domain.Commands;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.CommandHandlers
{
    internal abstract class CommandHandler
    {
        protected IStateService _stateService;
        public CommandHandler(IStateService stateService)
        {
            this._stateService = stateService;
        }
    }
}
