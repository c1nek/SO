using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;

namespace Memory
{
    
    public static class Mem
    {
        public static byte[] MEMORY = new byte[65536];
        public static void XA()
        {
            //allocate memory
        }
        public static void start()
        {
            //create FSB blocks (called only once by IPLRTN)
        }
    }

    

}
