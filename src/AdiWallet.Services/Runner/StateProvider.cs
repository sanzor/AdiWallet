using AdiWallet.Domain;
using AdiWallet.Services.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdiWallet.Services.Runner
{
    internal class StateProvider : IStateProvider
    {
        private readonly string _stateFilePath;
        public StateProvider(string stateFilePath)
        {
            _stateFilePath = stateFilePath ?? throw new ArgumentNullException(nameof(stateFilePath));
        }

        public EitherAsync<Error, AppState> LoadStateAsync()
        {
            return TryAsync(async () =>
            {

                if (!File.Exists(_stateFilePath))
                {
                    var newState = new AppState();
                    var content = JsonSerializer.Serialize(newState);

                    await File.WriteAllTextAsync(_stateFilePath, content);
                    return newState;
                }
                var stringContent = await File.ReadAllTextAsync(_stateFilePath);
                if (string.IsNullOrEmpty(stringContent))
                {
                    var emptyState = new AppState();
                    await File.WriteAllTextAsync(_stateFilePath, JsonSerializer.Serialize(emptyState));
                    return emptyState;
                }
                var state = JsonSerializer.Deserialize<AppState>(stringContent);
                return state;
            }).ToEither(err =>
            {
                return err;
            });

        }
        public EitherAsync<Error, Unit> WriteStateAsync(AppState state)
        {
            return TryAsync(async () =>
            {
                var content = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_stateFilePath, content);
                return Unit.Default;
            }).ToEither();
        }
    }
}
