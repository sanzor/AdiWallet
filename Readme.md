***AdiWallet*** - simple NFT wallet 

This is a console application that acts as an NFT wallet.
You run the app which receives some subset of
transactions, and processes them in such a way that enables the program to
answer questions about NFT ownership.
Thr program executes only a single command each time it is run, and
will persist state between runs.

The program supports :
- running transaction batch from a json file
- running transaction batch inline (single or array of transactions)
- reset wallet
- information about a NFT
- information about a Wallet
  For more info about the specifications check out the doc [here](https://github.com/sanzor/AdiWallet/blob/master/docs/documentation)

**Domain Schema**
![image](https://github.com/sanzor/AdiWallet/assets/26228414/9d943bc9-5baa-4f06-a331-e69d4e6a75d0)


**Notes:**

- I have made heavy usage of  a functional library [Louthy - functional programming library for C# ](https://github.com/louthy/language-ext) in order to make usage of Railway Programming (shortcircuit the flow at first error)

- Project structure
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
  ```

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
