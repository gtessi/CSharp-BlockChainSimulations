using BlockChainSimulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSimulation
{
    public class FileManager
    {
        public BlockChain ReadFile()
        {
            //BlockChain chain = new BlockChain(true);
            BlockChain chain = new BlockChain();

            string path = AppDomain.CurrentDomain.BaseDirectory;

            string file = path + "teslaBlockChain.vxf";

            if (File.Exists(file))
            {

            }

            return chain;
        }
    }
}
