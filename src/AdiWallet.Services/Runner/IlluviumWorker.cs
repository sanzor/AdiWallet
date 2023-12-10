
using AdiWallet.Domain;
using AdiWallet.Domain.Commands;
using AdiWallet.Services.CommandHandlers;
using AdiWallet.Services.Converters;
using AdiWallet.Services.Messages;
using AdiWallet.Services.Messages.Validators;

namespace AdiWallet.Services.Runner
{
    public class AdiWalletWorker
    {
        private readonly string _stateFilePath;
        public AdiWalletWorker(string stateFilePath)
        {
           _stateFilePath=stateFilePath ?? throw new ArgumentNullException(nameof(_stateFilePath));
        }

        public static EitherAsync<Error,Unit> RunAsync(AdiWalletRunnerParams @params)
        {
            var runner = new AdiWalletWorker(@params.StateFilePath);
            return runner.InnerRunAsync(@params.CommandLineArgs);
        }
        private EitherAsync<Error,Unit>  InnerRunAsync(string[] cmdLineArgs)
        {

            var stateProvider = new StateProvider(_stateFilePath);
            var result = 
                 RightAsync<RunnerContext, RunnerContext>(new RunnerContext { }) 
                .Bind(context=>LoadStateAsync(context,stateProvider))
                .Bind(context => SetRunningCommandAsync(context, cmdLineArgs))
                .Bind(CreateRunner)
                .Bind(RunCommandAsync)
                .MapLeft(context=>context.Error)
                .Bind(context=>SaveNewStateAsync(context,stateProvider));
            return result;
        }

        private EitherAsync<RunnerContext, RunnerContext> RunCommandAsync(RunnerContext context)
        {
            return context.Runner
                .RunAsync(context.Command)
                .Map(result =>
                {
                    context.CommandResult = result;
                    return context;
                }).MapLeft(context.SetError);
        }
        private EitherAsync<Error,Unit> SaveNewStateAsync(RunnerContext context,IStateProvider stateProvider)
        {
            return stateProvider.WriteStateAsync(context.CommandResult.CurrentState);
        }
       
        private EitherAsync<RunnerContext,RunnerContext> LoadStateAsync(RunnerContext context,IStateProvider stateProvider)
        {
            
            return stateProvider.LoadStateAsync()
                .Map(state =>
                {
                    context.OriginalState = state;
                    return context;
                })
                .MapLeft(context.SetError);

        }
        private EitherAsync<RunnerContext, RunnerContext> SetRunningCommandAsync(RunnerContext context, string[] cmdLineArgs)
        {

            var result =
                new CommandParser()
                .ParseCommand(cmdLineArgs)
                .Map(command =>
                {
                    context.Command = command;
                    return context;
                })
                .MapLeft(context.SetError)
                .ToAsync();
            return result;
        }

        private EitherAsync<RunnerContext,RunnerContext> CreateRunner(RunnerContext context)
        {

            State.IStateService stateService = new State.StateService(context.OriginalState);
            
            var messageParser = new MessageParserService(new MessageValidator());
            var handlers = new Dictionary<CommandKind, ICommandHandler>
            {
                { CommandKind.READ_INLINE, new ReadInlineHandler(stateService,messageParser) },
                { CommandKind.READ_FILE, new ReadFileHandler(stateService,messageParser) },
                { CommandKind.NFT_INFO, new NFTInfoHandler(stateService) },
                { CommandKind.WALLET_INFO, new WalletInfoHandler(stateService) },
                {CommandKind.RESET,new ResetHandler(stateService) }
            };
            var commandProcessor = new CommandProcessor(handlers);
            var runner = new Runner(commandProcessor);
            context.Runner = runner;
            return RightAsync<RunnerContext,RunnerContext>(context);

        }
    }
}
