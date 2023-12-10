using FluentAssertions;
using AdiWallet.Domain;
using AdiWallet.Domain.Commands;
using AdiWallet.Domain.Messages;
using AdiWallet.Domain.Wallet;
using AdiWallet.Services.Messages;
using AdiWallet.Services.Messages.Validators;
using AdiWallet.Services.Runner;
using LanguageExt;
using LanguageExt.Pipes;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Input;

namespace AdiWalletTests
{
    public class AdiWalletTests
    {
        Func<bool> CleanStateFile = () =>
        {
            File.Delete(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            STATE_FILE));
            return true;
        };

        Func<string, string> ToTargetDirPath = x =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            x);
        private const string JSON_FILE = "test.json";
        private const string STATE_FILE = "state.json";
        [Theory]
        [InlineData("{\"Type\": \"Mint\", \"TokenId\": \"0x...\", \"Address\": \"0yy\"}")]
        [InlineData("{\"Type\": \"Burn\", \"TokenId\": \"0x...\"}")]
        [InlineData("{\"Type\": \"Transfer\", \"From\": \"0x...\",\"To\": \"0x...\", \"TokenId\": \"0x...\"}")]

        public void CanDeserializeMessage(string data)
        {


            var options = new JsonSerializerOptions
            {
                Converters = { new MessageConverter { } }
            };
            if (data.StartsWith("["))
            {
                var a = JsonSerializer.Deserialize<List<Message>>(data, options);
            }
            else
            {
                var message = JsonSerializer.Deserialize<Message>(data, options);
                var c = new List<Message> { message };
            }
        }


        [Theory]
        [InlineData("[" +
            "{\"Type\": \"Mint\", \"TokenId\": \"a \", \"Address\": \"adi\"}," +
            "{\"Type\": \"Transfer\", \"From\": \"adi\",\"To\": \"dan\", \"TokenId\": \"a\"}," +
            "{\"Type\": \"Burn\", \"TokenId\": \"a\"}]")]
        public void CanDeserializeMessages(string data)
        {


            var options = new JsonSerializerOptions
            {
                Converters = { new MessageConverter { } }
            };
            if (data.StartsWith("["))
            {
                var a = JsonSerializer.Deserialize<List<Message>>(data, options);
            }
            else
            {
                var message = JsonSerializer.Deserialize<Message>(data, options);
                var c = new List<Message> { message };
            }
        }
        IEnumerable<Message> DeserializeMessagesUtil(string data)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    Converters = { new MessageConverter { } }
                };
                if (data.StartsWith("["))
                {

                    return JsonSerializer.Deserialize<List<Message>>(data, options);
                }
                var message = JsonSerializer.Deserialize<Message>(data, options);
                return new List<Message> { message };
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        [Fact]
        public async Task CanDeserializeMessageFromFile()
        {
            var jsonFilePath = ToTargetDirPath(JSON_FILE);
            var tokenId = "mytoken";
            var from = "from";
            var to = "to";
            var message = new Transfer { From = from, To = to, TokenId = tokenId };
            var messageString = JsonSerializer.Serialize(message);
            await File.WriteAllTextAsync(jsonFilePath, messageString);
            var data = "{\"Kind\":2,\"From\":\"from\",\"To\":\"to\",\"TokenId\":\"mytoken\"}";//await File.ReadAllTextAsync(jsonFilePath);
            var messageList = DeserializeMessagesUtil(data);
        }
        [Fact]
        public async Task CanDeserializeMessagesFromFile()
        {
            var jsonFilePath = ToTargetDirPath(JSON_FILE);
            var tokenId = "mytoken";
            var from = "from";
            var to = "to";
            var message = new Transfer { From = from, To = to, TokenId = tokenId };
            var messageString = JsonSerializer.Serialize(message);
            await File.WriteAllTextAsync(jsonFilePath, messageString);
            var data = await File.ReadAllTextAsync(jsonFilePath);
            var messageList=DeserializeMessagesUtil(data);
        }

        [Theory]
        [InlineData("--read-inline", "{\"Type\": \"Mint\", \"TokenId\": \"0x...\", \"Address\":\"myadr\"}")]
        public async Task CanProcessInline(params string[] cmdLineArgs)
        {
            var stateFilePath = ToTargetDirPath(STATE_FILE);
            CleanStateFile();
            var rez = await AdiWalletWorker.RunAsync(
                new()
                {
                    CommandLineArgs = cmdLineArgs,
                    StateFilePath = stateFilePath
                });
            rez.IsRight.Should().Be(true);
        }


        [Theory]
        [InlineData("--read-inline",
            "[{\"Type\": \"Mint\", \"TokenId\": \"0x\",\r\n\"Address\": \"0x\"},{\"Type\": \"Burn\", \"TokenId\": \"0x\"}]")]
        public async Task CanProcessInlineArray(params string[] cmdLineArgs)
        {
            var stateFilePath = ToTargetDirPath(STATE_FILE);
            CleanStateFile();
            var rez = await AdiWalletWorker.RunAsync(new()
            {
                CommandLineArgs = cmdLineArgs,
                StateFilePath = stateFilePath
            });
            rez.IsRight.Should().Be(true);

        }


        [Theory]
        [InlineData("--read-file", JSON_FILE, "[" +
            "{\"Type\": \"Mint\", \"TokenId\": \"ab\", \"Address\": \"adi\"}," +
            "{\"Type\": \"Transfer\", \"From\": \"adi\",\"To\": \"dan\", \"TokenId\": \"ab\"}," +
            "{\"Type\": \"Burn\", \"TokenId\": \"ab\"}]")]
        public async Task CanProcessJson(params string[] cmdLineArgs)
        {
            ///cleaning old state if any
            CleanStateFile();
            var messages = DeserializeMessages(cmdLineArgs[2]);

            string jsonFilePath = ToTargetDirPath(cmdLineArgs[1]);
            string stateFilePath = ToTargetDirPath(STATE_FILE);
            //preparing the input json file based on inlinedata
            await WriteMessagesToJsonFileAsync(messages, jsonFilePath);

            //running the worker
            var rez = await AdiWalletWorker.RunAsync(new()
            {
                StateFilePath = stateFilePath,
                CommandLineArgs = new[] { cmdLineArgs[0], jsonFilePath }
            });

            //fetching the new state of the program
            var stateString = await File.ReadAllTextAsync(ToTargetDirPath(STATE_FILE));
            var state = JsonSerializer.Deserialize<AppState>(stateString);

            // after a mint , transfer and burn on the same token there should be no nfts left
            state.NftMap.Count.Should().Be(0);


        }

       
        [Fact]
        public async Task CanHandleTransferOnExistingState()
        {
            CleanStateFile();
            ///setup existing state
            string stateFilePath = ToTargetDirPath(STATE_FILE);
            var tokenId = "mytoken";
            var from = "from";
            var to = "to";
            var oldState = new AppState();
            var wallet = new Wallet { Address = from };
            var nft = new NFT { TokenId = tokenId, Address = wallet.Address };
            wallet.Nfts.Add(nft.TokenId, nft);
            oldState.NftMap.Add(nft.TokenId, nft);
            oldState.WalletMap.Add(wallet.Address, wallet);
            await File.WriteAllTextAsync(stateFilePath, JsonSerializer.Serialize(oldState));

            //run the transfer command inline
            var message = new Transfer { From = from, To = to, TokenId = tokenId };
            var messageString = JsonSerializer.Serialize(message);
            var rez = await AdiWalletWorker.RunAsync(
               new()
               {
                   CommandLineArgs = new[] { "--read-inline", messageString },
                   StateFilePath = stateFilePath
               });
            rez.IsRight.Should().BeTrue();
            ///retrieving state after program ran
            var stateString = await File.ReadAllTextAsync(stateFilePath);
            var state = JsonSerializer.Deserialize<AppState>(stateString);

            //checking if new state is correct
            var toWalletExists = state.WalletMap.TryGetValue(to, out var toWallet);
            var fromWalletExists = state.WalletMap.TryGetValue(from, out var fromWallet);
            toWalletExists.Should().BeTrue();
            fromWalletExists.Should().BeTrue();
            fromWallet.Nfts.Count.Should().Be(0);
            toWallet.Nfts.Count.Should().Be(1);

        }
        [Fact]
        public async Task CanHandleReadWalletInfo()
        {
            var address = "myaddress";
            var tokenId = "tokenId";
            var appState = new AppState();
            var wallet = new Wallet { Address = address };
            var token = new NFT { Address = address, TokenId = tokenId };
            wallet.Nfts.Add(token.TokenId, token);
            appState.NftMap.Add(token.TokenId, token);
            appState.WalletMap.Add(wallet.Address, wallet);
            string stateFilePath = ToTargetDirPath(STATE_FILE);
            var stateString=JsonSerializer.Serialize(appState);
            await File.WriteAllTextAsync(stateFilePath, stateString);
            bool ok = true;
            var rez = await AdiWalletWorker.RunAsync(
               new()
               {
                   CommandLineArgs = new[] { "--wallet", wallet.Address },
                   StateFilePath = stateFilePath
               })
                .MapLeft(err =>
                {
                    ok = false;
                    return err;
                });
            ok.Should().BeTrue();
        }

        [Fact]
        public async Task CanHandleNftInfo()
        {
            var address = "myaddress";
            var tokenId = "tokenId";
            var appState = new AppState();
            var wallet = new Wallet { Address = address };
            var token = new NFT { Address = address, TokenId = tokenId };
            wallet.Nfts.Add(token.TokenId, token);
            appState.NftMap.Add(token.TokenId, token);
            appState.WalletMap.Add(wallet.Address, wallet);
            string stateFilePath = ToTargetDirPath(STATE_FILE);
            var stateString = JsonSerializer.Serialize(appState);
            await File.WriteAllTextAsync(stateFilePath, stateString);
            bool ok = true;
            var rez = await AdiWalletWorker.RunAsync(
               new()
               {
                   CommandLineArgs = new[] { "--nft",token.TokenId },
                   StateFilePath = stateFilePath
               })
                .MapLeft(err =>
                {
                    ok = false;
                    return err;
                });
            ok.Should().BeTrue();
        }

        private async Task WriteMessagesToJsonFileAsync(IEnumerable<Message> messages, string filename = JSON_FILE)
        {


            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            var messagesString = JsonSerializer.Serialize(messages);

            await File.WriteAllTextAsync(filename, messagesString);

        }

        private List<Message> DeserializeMessages(string content)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new MessageConverter { } }
            };
            if (content.StartsWith("["))
            {
                return JsonSerializer.Deserialize<List<Message>>(content, options);
            }
            var message = JsonSerializer.Deserialize<Message>(content, options);
            return new List<Message> { message };
        }



    }
}
