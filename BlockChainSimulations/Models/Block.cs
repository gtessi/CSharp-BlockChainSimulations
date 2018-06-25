using BlockChainSimulation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSimulation.Models
{
    public class Block
    {
        public int Index { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public string Hash { get; set; }

        public string PreviousHash { get; set; }
        
        public DateTime TimeStamp { get; set; }

        public int Nonce { get; set; }


        public Block(IList<Transaction> transactions, string previousHash)
        {
            this.Index = 0;
            this.Transactions = transactions;
            this.Hash = this.CalculateHash();
            this.PreviousHash = previousHash;
            this.TimeStamp = DateTime.Now;
            this.Nonce = 0;
        }


        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");

            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
