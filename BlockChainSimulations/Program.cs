using BlockChainSimulation.Models;
using BlockChainSimulation.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSimulation
{
    /// <summary>
    /// Block Chain Simulation
    /// Based on Henry He's article "Building A Blockchain In .NET Core"
    /// Source: https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/
    /// </summary>
    class Program
    {
        public static int Port = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();

        public static BlockChain TeslaCoin = new BlockChain();

        public static string Name = "Unknown";

        static void Main(string[] args)
        {
            TeslaCoin.InitializeChain();

            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                Name = args[1];

            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }
            if (Name != "Unkown")
            {
                Console.WriteLine($"Current user is {Name}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Display Blockchain");
            Console.WriteLine("4. Exit");
            Console.WriteLine("--------------------------------------------------");

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        string serverURL = Console.ReadLine();
                        Client.Connect($"{serverURL}/Blockchain");
                        break;
                    case 2:
                        Console.WriteLine("Please enter the receiver name");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        string amount = Console.ReadLine();
                        TeslaCoin.CreateTransaction(new Transaction(Name, receiverName, int.Parse(amount)));
                        TeslaCoin.ProcessPendingTransactions(Name);
                        Client.Broadcast(JsonConvert.SerializeObject(TeslaCoin));
                        break;
                    case 3:
                        Console.WriteLine("Blockchain");
                        Console.WriteLine(JsonConvert.SerializeObject(TeslaCoin, Formatting.Indented));
                        break;
                }

                Console.WriteLine("Please select an action:");

                string action = Console.ReadLine();

                selection = int.Parse(action);
            }

            Client.Close();
        }
    }
}
