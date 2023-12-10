**AdiWallet** - simple NFT wallet 

This is a console application that acts as an NFT wallet.
You run the app which receives some subset of
transactions, and processes them in such a way that enables the program to
answer questions about NFT ownership.
Thr program executes only a single command each time it is run, and
will persist state between runs.

Domain
![image](https://github.com/sanzor/AdiWallet/assets/26228414/9d943bc9-5baa-4f06-a331-e69d4e6a75d0)

The project was done using .NET 6 , C#  and Visual Studio 2022 as a tool

The architecture is found in folder: docs 

**Notes:**

- I have made heavy usage of  a functional library [LanguageExt]([louthy/language-ext: C# functional language extensions - a base class library for functional programming (github.com)](https://github.com/louthy/language-ext)) in order to make usage of Railway Programming (shortcircuit the flow at first error)
- I have separated the project into 2 folders
  - src:
    - AdiWallet.App  - the console application
    - AdiWallet.Services - the place where logic logic resides
    - AdiWallet.Domain - the place where the domain model is placed ( all domain objects)
      - Commands - NftInfo, WalletInfo, Reset, ReadFile , ReadInline
      - Messages - Burn, Mint ,Transfer
      - Domain objects - Wallet , NFT , AppState (this one holds the state of the app and gets flushed to disk at the end of the program)
  - tests
    - AdiWallet.Tests - contains XUnit tests for most of the logic
      - tests for deserializing the messages
      - tests for running one message , a batch of messages
      - tests for the other commands , and testing if the resulting state written on disk is right
- Inspiration :
  I had quite a big of a headache getting to deserialize the messages right , therefore i used this implementation of a custom serializer:
  ```
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
  ```



I would deserialize messages like this :

```
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
```

Unfortunately i did not have enough time to test it thoroughly as well as setting up completely for MAC OS and Windows. That would've been hardcore for one day....for 4 hours thats not even enough to get the Domain and understand the problem ok.

This is 4 hours of work in one day and another 6 hours in the next.


**Setting up:**

- Windows:
  - `dotnet publish -c Release -r win10-x64 -o `

Go to the [target-folder] and open a Command Prompt terminal 

Run :   AdiWallet.App.exe [arguments]

 EX:  

    `AdiWallet.App.exe --nft somenft`

    `AdiWallet.App.exe --wallet somewallet`

    `AdiWallet.App.exe --read-file myfile.json`

    `AdiWallet.App.exe --read-inline "{\"Type\": \"Mint\", \"TokenId\": \"ab\", \"Address\": \"adi\"},{\"Type\": \"Transfer\", \"From\": \"adi\",\"To\": \"dan\", \"TokenId\": \"ab\"},{\"Type\": \"Burn\", \"TokenId\": \"ab\"}]"`

    `Illuviu.App.exe reset`



The project contains unit tests which can be run by going in the root directory of the source folder and running :

`dotnet test`
