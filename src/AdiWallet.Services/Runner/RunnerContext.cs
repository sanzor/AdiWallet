using AdiWallet.Domain;
using AdiWallet.Domain.Commands;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.Runner
{
    /// <summary>
    /// Class holding the state along the different steps of the flow
    /// </summary>
    internal class RunnerContext
    {
        public Command Command { get; set; }

        public AppState OriginalState { get; set; }
        
        /// <summary>
        /// This fields will hold the error that will end the flow (short circuit)
        /// </summary>
        public Error Error { get; set; }
        public IRunner Runner { get; set; }


        /// <summary>
        /// Output
        /// </summary>
        public CommandResult CommandResult { get; set; }
    }
    internal static class RunnerContextExtensions
    {
        public static RunnerContext SetError(this RunnerContext context,Error error)
        {
            context.Error = error;
            return context;
        }
    }
}
