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
    class SVC
    {
        //Na samym początku Tworzy procesy *IN i *OUT
        //Poprzez Komunikację z Procesem *IN szuka karty (linijki) $JOB





        
    }

   static public class IPLRTN
   {
       static void Main()
       {
           Mem.start();//całą pamięć wolną opisuje przy pomocy bloków FSB
           //Tworzy procesy SVC dla każdego strumienia zleceń (2x)
           Proc.XC("*IPSUB");
       }
   }
}
