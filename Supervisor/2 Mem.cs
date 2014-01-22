using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;
using Processor;

namespace Memory
{
    
    public static class Mem
    {
        public static FSB FSBPTR=null;
        public static SEMAPHORE MEMORY_SEM=new SEMAPHORE();//semafor z domyslna wartoscia 0
        public static SEMAPHORE FSBSEM = new SEMAPHORE(1);//semafor wyłączności dostępu do listy bloków FSB
        public static byte[] MEMORY = new byte[65536];

        public static byte[] XA=new byte[]
        {
            
        };

        public static void XB()
        {
        }
        public static void start()
        {
            //create FSB blocks (called only once by IPLRTN)
        }
    }

    public class FSB
    {
        FSB NEXT;
        int pocz;
        int koniec;
        int wielkosc;
    }
    

}
