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
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART, SEM };
        public enum wartosc_SEM : byte { MEMORY };
        public enum wartosc_METHOD : byte { CZYSC_PODR };
        //Na samym początku Tworzy procesy *IN i *OUT
       
        //Poprzez Komunikację z Procesem *IN szuka karty (linijki) $JOB
        //Pamięć wstępna. Z niej ładowane do pamięci głównej
        private static byte[] mem = new byte[]{
        /*0000*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.SEM,      (byte)wartosc_SEM.MEMORY,       //zapisanie semafora pamięci w 2 rejestrze
        /*0003*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.P,                                        //wywołanie operacji P na semaforze w 2 rejestrze (semafor pamięci)
        
        /*0005*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R3,       (byte)wartosc_TYP.WART,1,0,     //wpisanie do rejestru wartości 8
        /*000A*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.E,                                        //wywołanie operacji przydziału pamięci roboczej o wielkości określonej w rejestrze 3. Operacja zwraca adres pamięci roboczej w rejestrze 3.
        
        /*000C*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.V,                                        //wywołanie operacji V na semaforze w 2 rejestrze (semafor pamięci)
        
        /*000E*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R3,           //przepisanie adresu pamięci roboczej do rejestru 1
        /*0011*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'*',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*0014*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /*0016*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'I',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*0019*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /*001B*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'N',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*001E*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /*0020*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.R3,           //wpisanie adresu pamięci roboczej do rejestru 2
        /*0023*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,                                        //wywołanie programu tworzącego proces (program pobiera nazwę procesu z rejestru 2)
        
        /*0025*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,8,
        /*002A*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,1,
        /*002E*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /*0030*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,0,       //wpisanie adresu uruchamianego programu (w tym wypadku *IN) jest to adres 256 + adres początku 
        /*xxxx*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,0,     //ustawienie rejestru 1 tak by wskazywał adres urządzenia
        /*0033*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,                                        //uruchomienie procesu
 
        /*0035*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R3,           //przepisanie adresu pamięci roboczej do rejestru 1
        /*0038*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'*',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*003B*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /*003D*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'O',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*0040*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /*0042*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'U',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*0045*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /*0047*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'T',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /*004A*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.R3,           //wpisanie adresu pamięci roboczej do rejestru 2
        /*004D*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,                                        //wywołanie programu tworzącego proces (program pobiera nazwę procesu z rejestru 2)
        
        /*004F*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,8,
        /*0054*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,1,
        /*0058*/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /*005A*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,0,       //wpisanie adresu uruchamianego programu (w tym wypadku *IN) jest to adres 256 + adres początku procesu *IBSUB
        /*005E*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,0,
        /*005E*/    (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,                                        //uruchomienie procesu

        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R3,           //przepisanie adresu pamięci roboczej do rejestru 1
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'*',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'I',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                       //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'N',                      //wpisanie znaku do komórki pamięci w rejestrze 1
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,       
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,8,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      8,                             //określenie długości komunikatu
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'R',                      //zapisanie komunikatu
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'E',
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'A',
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'D',
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'0',
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'0',
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'0',
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)'1',

                (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,                                        //wysłanie komunikatu wskazywanego przez reg 2

                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.CZYSC_PODR, (byte)wartosc_TYP.R3, (byte)wartosc_TYP.WART, (byte)1, (byte)0,

                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,8,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,32,

        /*,"","","","",""*/};





        public static void zaladuj(int m)
        {
            for (int i = 0; i < mem.Length; i++)
            {
                Mem.MEMORY[i+m] = mem[i];
            }
        }

        public static void czyscPodr(int adr, int rozmiar)
        {
            for (int i = 0; i < rozmiar; i++)
            {
                Mem.MEMORY[adr + i] = 0;
            }
        }

        
    }

   static public class IPLRTN
   {
       static void Main()
       {
          
           //tworzy swój PCB dodaje go na listę ustaiwa wszystkie wartości by wskazywały na niego
           PCB iplrtn = new PCB("*IPRTLN",0);
           rejestry.r2=iplrtn;
           zawiadowca.RUNNING = iplrtn;
           zawiadowca.NEXTTRY = iplrtn;
           iplrtn.LAST_PCB_ALL = iplrtn;
           iplrtn.LAST_PCB_GROUP = iplrtn;
           iplrtn.NEXT_PCB_ALL = iplrtn;
           iplrtn.NEXT_PCB_GROUP = iplrtn;

           IBSUB.zaladuj(256);
           IBSUB.zaladuj(1280);

           Mem.start();//całą pamięć wolną opisuje przy pomocy bloków FSB i wszystkie klucze ochrony ustawia na 0

           PCB ibsub1 = new PCB("*IBSUB",100);
           ibsub1.cpu_stan[0] = 0;
           ibsub1.cpu_stan[1] = 0;
           ibsub1.cpu_stan[2] = 0;
           ibsub1.cpu_stan[3] = 0;
           ibsub1.cpu_stan[4] = 256;

           PCB ibsub2 = new PCB("*IBSUB",100);
           ibsub2.cpu_stan[0] = 0;
           ibsub2.cpu_stan[1] = 0;
           ibsub2.cpu_stan[2] = 0;
           ibsub2.cpu_stan[3] = 0;
           ibsub2.cpu_stan[4] = 1280;

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
