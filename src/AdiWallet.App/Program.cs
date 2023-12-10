// See https://aka.ms/new-console-template for more information


using AdiWallet.App;
using AdiWallet.Services.Runner;
using System.Reflection;

Console.WriteLine("Hello, World!");
if (args.Length > 0)
{

    var stateFilePath = Path.Combine(Constants.STATE_FILE,
        Directory.GetParent(Assembly.GetExecutingAssembly().FullName).FullName);
    var rez = await AdiWalletWorker.RunAsync(new()
    {
        CommandLineArgs = args,
        StateFilePath =stateFilePath
    });




}
