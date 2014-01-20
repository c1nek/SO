using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;
using Process;
using Memory;
using External;

namespace Supervisor
{
    public static class IBSUB
    {
        //Na samym początku Tworzy procesy *IN i *OUT
       
        //Poprzez Komunikację z Procesem *IN szuka karty (linijki) $JOB
        public static string[] MEMORY_IBSUB = new string[100];

        



        
    }

   static public class IPLRTN
   {
       public string[] MEMORY_IPLRTN = new string[100];
       static void Main()
       {
          
           //tworzy swój PCB dodaje go na listę ustaiwa wszystkie wartości
           PCB iplrtn = new PCB("*IPRTLN", 0, 0);
           rejestry.set_r2(iplrtn);
           Proc.XI();


           Mem.start();//całą pamięć wolną opisuje przy pomocy bloków FSB
           //Tworzy procesy IBSUB dla każdego strumienia zleceń (2x)
           //Korzysta z interpretera
           
       }
   }
}
