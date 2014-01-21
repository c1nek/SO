using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;
using Process;
using Memory;
using External;
using Interpreter;

namespace Supervisor
{
    public static class IBSUB
    {
        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, METHOD, CREATE };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_CREATE : byte { KOM, PCB };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART };
        public enum wartosc_SEM : byte { MEMORY };
        //Na samym początku Tworzy procesy *IN i *OUT
       
        //Poprzez Komunikację z Procesem *IN szuka karty (linijki) $JOB
        //Pamięć wstępna. Z niej ładowane do pamięci głównej
        private static byte[] mem = new byte[]{
        /*0000*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_SEM.MEMORY,       //zapisanie semafora pamięci w 2 rejestrze
        /*0003*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.P,                                        //wywołanie operacji P na semaforze w 2 rejestrze
        /*0005*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R3,       (byte)wartosc_TYP.WART,0,0,8,   //wpisanie do rejestru wartości 8
        /*000A*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.E,                                        //wywołanie operacji przydziału pamięci roboczej o wielkości określonej w rejestrze 3. Operacja zwraca adres pamięci roboczej w rejestrze 3.
        /*000C*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.V,
        /*000E*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.R3,
        /*"SVC *IN","SVC Y *IN","SVC C *OUT","SVC Y *OUT","REG2 KOM(*IN,READxxxx)","","","","",""*/};


        public static void zaladuj(int m)
        {
            for (int i = 0; i < mem.Length; i++)
            {
                Mem.MEMORY[i+m] = mem[i];
            }
        }



        
    }

   static public class IPLRTN
   {
       public string[] MEMORY_IPLRTN = new string[100];
       static void Main()
       {
          
           //tworzy swój PCB dodaje go na listę ustaiwa wszystkie wartości by wskazywały na niego
           PCB iplrtn = new PCB("*IPRTLN");
           rejestry.set_r2(iplrtn);
           zawiadowca.RUNNING = iplrtn;
           zawiadowca.NEXTTRY = iplrtn;
           iplrtn.LAST_PCB_ALL = iplrtn;
           iplrtn.LAST_PCB_GROUP = iplrtn;
           iplrtn.NEXT_PCB_ALL = iplrtn;
           iplrtn.NEXT_PCB_GROUP = iplrtn;


           Mem.start();//całą pamięć wolną opisuje przy pomocy bloków FSB i wszystkie klucze ochrony ustawia na 0

           PCB ibsub1 = new PCB("*IBSUB");
           ibsub1.cpu_stan[0] = 0;
           ibsub1.cpu_stan[1] = 0;
           ibsub1.cpu_stan[2] = 0;
           ibsub1.cpu_stan[3] = 0;
           ibsub1.cpu_stan[4] = 100;

           PCB ibsub2 = new PCB("*IBSUB");
           ibsub2.cpu_stan[0] = 0;
           ibsub2.cpu_stan[1] = 0;
           ibsub2.cpu_stan[2] = 0;
           ibsub2.cpu_stan[3] = 0;
           ibsub2.cpu_stan[4] = 200;

           iplrtn.NEXT_PCB_ALL = ibsub1;
           iplrtn.LAST_PCB_ALL = ibsub2;
           
           ibsub1.NEXT_PCB_ALL = ibsub2;
           ibsub2.NEXT_PCB_ALL = iplrtn;
           
           ibsub1.LAST_PCB_ALL = ibsub2;
           ibsub2.LAST_PCB_ALL = ibsub2;

           ibsub1.NEXT_PCB_GROUP = ibsub1;
           ibsub2.NEXT_PCB_GROUP = ibsub2;

           ibsub1.LAST_PCB_GROUP = ibsub1;
           ibsub2.LAST_PCB_GROUP = ibsub2;
           //Tworzy procesy IBSUB dla każdego strumienia zleceń (2x)
           //Korzysta z interpretera
           
       }
   }
}
