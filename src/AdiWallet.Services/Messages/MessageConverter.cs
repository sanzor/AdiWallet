using AdiWallet.Domain.Messages;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AdiWallet.Services.Messages
{
    public class MessageConverter : JsonConverter<Message>
    {
        public override Message Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;

                if (root.TryGetProperty("Type", out JsonElement typeElement) && typeElement.ValueKind == JsonValueKind.String)
                {
                    string messageType = typeElement.GetString();

                    switch (messageType)
                    {
                        case "Mint":
                            return JsonSerializer.Deserialize<Mint>(root.GetRawText(), options);
                        case "Burn":
                            return JsonSerializer.Deserialize<Burn>(root.GetRawText(), options);
                        case "Transfer":
                            return JsonSerializer.Deserialize<Transfer>(root.GetRawText(), options);
                            // Add cases for other message types if needed
                    }
                }

                // Default to deserializing as the base Message type
                return JsonSerializer.Deserialize<Message>(root.GetRawText(), options);
            }
        }

        public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
