using AdiWallet.Domain;
using AdiWallet.Domain.Commands;
using AdiWallet.Domain.Messages;
using AdiWallet.Services.Messages;
using AdiWallet.Services.State;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdiWallet.Services.CommandHandlers
{
    internal class ReadInlineHandler : CommandHandler, ICommandHandler
    {
        private readonly IMessageParserService _messageParser;
        public ReadInlineHandler(IStateService stateService, IMessageParserService messageParser) : base(stateService)
        {
            _messageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
        }

        public CommandKind Kind => CommandKind.READ_INLINE;

        public EitherAsync<Error, CommandResult> RunCommandAsync(Command command)
        {
            if (command.CommandKind is not CommandKind.READ_INLINE)
            {
                return LeftAsync<Error, CommandResult>(Error.New("Invalid command"));
            }
            var result =
                Try(() =>
            {
                var cmd = command as ReadInline;
                return cmd.Data;
            })
          .ToEither(Error.New)
          .Bind(_messageParser.ParseMessages)
          .Bind(_stateService.ApplyMessages)
          .Bind(_=>_stateService.GetNewestState())
          .Map(state=>new CommandResult { CurrentState=state})
          .ToAsync();
            return result;
        }
    }
}
