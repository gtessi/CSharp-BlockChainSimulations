using BlockChainSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSimulation
{
    public class BlockChain
    {
        public IList<Block> Chain { get; set; }

        public int Difficulty { set; get; }


        public IList<Transaction> PendingTransactions = new List<Transaction>();

        public int Reward { get; set; }


        public BlockChain() { }

        public void InitializeChain()
        {
            this.Chain = new List<Block>();

            this.AddGenesisBlock();

            this.Difficulty = 2;

            this.Reward = 1;
        }
        
        public Block CreateGenesisBlock()
        {
            Block genesis = new Block(new List<Transaction>(), null);

            genesis.Mine(this.Difficulty);
            PendingTransactions = new List<Transaction>();

            return genesis;
        }

        public void AddGenesisBlock()
        {
            this.Chain.Add(this.CreateGenesisBlock());
        }

        public Block GetLastBlock()
        {
            Block last = this.Chain.Last();

            return last;
        }

        public void AddBlock(Block block)
        {
            Block last = this.GetLastBlock();

            block.Index = last.Index + 1;
            block.PreviousHash = last.Hash;

            //block.Hash = block.CalculateHash(); // Hash is calculated inside Mine

            block.Mine(this.Difficulty);

            this.Chain.Add(block);
        }

        public bool IsValid()
        {
            bool result = true;

            int i = 1;
            while (i < this.Chain.Count && result)
            {
                Block current = Chain[i];
                Block previous = Chain[i - 1];

                if (current.Hash != current.CalculateHash())
                    result = false;

                if (current.PreviousHash != previous.Hash)
                    result = false;

                i++;
            }

            return result;
        }

        public void CreateTransaction(Transaction transaction)
        {
            this.PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            Block last = this.GetLastBlock();
            Block block = new Block(this.PendingTransactions, last.Hash);

            this.AddBlock(block);

            this.PendingTransactions = new List<Transaction>();
            this.CreateTransaction(new Transaction(null, minerAddress, this.Reward));
        }

        public int GetBalance(string minerAddress)
        {
            int result = 0;

            for (int i = 0; i < this.Chain.Count; i++)
            {
                for (int j = 0; j < this.Chain[i].Transactions.Count; j++)
                {
                    Transaction transaction = this.Chain[i].Transactions[j];

                    if (transaction.FromAddress == minerAddress)
                    {
                        result -= transaction.Amount;
                    }

                    if (transaction.ToAddress == minerAddress)
                    {
                        result += transaction.Amount;
                    }
                }
            }
            
            //result = this.PendingTransactions.Sum(x => x.ToAddress.Equals(minerAddress) ? x.Amount : 0);

            return result;
        }
    }
}
