using AdiWallet.Domain.Commands;
using AdiWallet.Domain.Messages;
using AdiWallet.Services.Messages;
using AdiWallet.Services.State;
using Newtonsoft.Json;

namespace AdiWallet.Services.CommandHandlers
{
    internal class ReadFileHandler : CommandHandler,ICommandHandler
    {
        private readonly IMessageParserService _messageParser;
        public ReadFileHandler(IStateService _state,IMessageParserService messageParser) : base(_state)
        {
            _messageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
        }

        public CommandKind Kind => CommandKind.READ_FILE;

        public EitherAsync<Error, CommandResult> RunCommandAsync(Command command)
        {
            if(command.CommandKind is not CommandKind.READ_FILE)
            {
                return LeftAsync<Error, CommandResult>(Error.New("Invalid command"));
            }
            var result=TryAsync(async () =>
            {
                var cmd = command as ReadFile;
                var data = await File.ReadAllTextAsync(cmd.FilePath);
                return data;
            }).ToEither()
            .Bind(msgString=>_messageParser.ParseMessages(msgString).ToAsync())
            .Map(_stateService.ApplyMessages)
            .Bind(_=>_stateService.GetNewestState().ToAsync())
            .Map(state=>new CommandResult { CurrentState = state });
            return result;

        }
    }
}
