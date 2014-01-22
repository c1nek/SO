using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;

namespace Process
{
    public static class Proc
    {

        public static void XC()
        {

        }
        public static void XI()
        {

        }
        
    }

    public class MESSAGE
    {
       public PCB SENDER;
       public MESSAGE NEXT;
       public int SIZE;
       public byte[] TEXT;
       public MESSAGE()
        {
            SENDER = null;
            NEXT = null;
            SIZE = 0;
            TEXT = null;
        }
    }
    
    
        
        
        

    
}
