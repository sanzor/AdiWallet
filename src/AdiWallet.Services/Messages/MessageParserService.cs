using AdiWallet.Domain.Messages;
using AdiWallet.Services.Messages.Validators;
using System.Text.Json;

namespace AdiWallet.Services.Messages
{
    internal class MessageParserService : IMessageParserService
    {
        private readonly IMessageValidator _messageValidator;
        public Either<Error, List<Message>> ParseMessages(string data)
        {
            return Try(() =>
            {
                
                var options = new JsonSerializerOptions
                {
                    Converters = { new MessageConverter { } }
                };
                if (data.StartsWith("["))
                {
                    
                    var msgs= JsonSerializer.Deserialize<List<Message>>(data, options);
                    return msgs;
                }
                var message = JsonSerializer.Deserialize<Message>(data, options);
                return new List<Message> { message };
            }).ToEither(exc =>
            {
                return Error.New(exc);
            })
            .Bind(ValidateMessages);

        }
        private Either<Error, List<Message>> ValidateMessages(List<Message> messages)
        {

            var validateMessagesResult = messages.Aggregate(true,
             (accu, message) => _messageValidator.ValidateMessage(message) && accu);
            return validateMessagesResult ?
                Right<Error, List<Message>>(messages) :
                Left<Error,List<Message>>(Error.New("Invalid message(s)"));
           
        }
        public MessageParserService(IMessageValidator validator)
        {
            _messageValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }
    }
}
