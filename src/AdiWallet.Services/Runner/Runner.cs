using AdiWallet.Services;
using LanguageExt.Common;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AdiWallet.Services.State;
using Newtonsoft.Json;
using Newtonsoft.Json;
using AdiWallet.Domain.Commands;

namespace AdiWallet.Services.Runner;

internal class Runner:IRunner
{
    private ICommandProcessor _commandProcessor;
    
   
    public EitherAsync<Error, CommandResult> RunAsync(Command command)
    {
        return _commandProcessor.RunCommandAsync(command);
    }
    public Runner(ICommandProcessor processor)
    {
        _commandProcessor = processor ?? throw new ArgumentNullException(nameof(processor));
    }
  
}
