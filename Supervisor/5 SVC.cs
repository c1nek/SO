﻿using System;
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
        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, JUMPF, JUMPR, JUMP, METHOD, FLAG };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART, SEM };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN };
       
        //Pamięć wstępna. Z niej ładowane do pamięci głównej
        private static byte[] mem = new byte[]{
        
        /*0000*/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.LR,
                    (byte)rozkaz.ADD,       (byte)wartosc_TYP.WART,     11,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.R0,
                    (byte)rozkaz.JUMPF,     (byte)wartosc_TYP.WART,     7,
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.A,
                    1,0,0,0,4,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.R2,
                    (byte)rozkaz.ADD,       (byte)wartosc_TYP.WART,     2,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R0,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R3,       (byte)wartosc_TYP.MEM,


        
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
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)0,//
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)0,
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,(byte)0,
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.R3,
                (byte)rozkaz.ADD,       (byte)wartosc_TYP.WART,     32,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.R0,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,                                        //wysłanie komunikatu wskazywanego przez reg 2 oczekiwanie na karte job pod adresem 32 pamięci roboczej

                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.CZYSC_PODR, (byte)wartosc_TYP.R3, (byte)wartosc_TYP.WART, (byte)1, (byte)0,

                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.PRZYG_XR,  (byte)wartosc_TYP.R2, (byte)wartosc_TYP.WART, 32,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.R,
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.INTER_KOM,

                (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,

                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.SCAN,


                
        /**/    

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
            rejestry.r8 = (Int16)(int)rejestry.r3;//zapisanie roczatku pamięci roboczej do 8 rejestru
        }

        public static void przygXR(int adr, int dl)
        {
            int tmp = adr;
            tmp += 8;
            Mem.MEMORY[tmp] =(byte) dl;
        }

        public static void interKom()
        {
            int i = (int)rejestry.r2;
            int j;
            string nazwa=null;
            string komunikat=null;

            for (; i < (int)rejestry.r2+8; i++)
            {

                if (Mem.MEMORY[i] != 0)
                {
                    nazwa += (char)Mem.MEMORY[i];//???? nie wiem czy działa
                }
            }
            if (nazwa != "*IN")
            {
                System.Console.WriteLine("Blad: zly nadawca. Oczekiwana wartosc to *IN. Otrzymano {0}", nazwa);
                Console.ReadLine();
                Environment.Exit(0);
            }
            int wielkosc = Mem.MEMORY[i];//wielkosc komunikatu
            j = i;
            for (; i < j + wielkosc; i++)
            {
                if (Mem.MEMORY[i] != 0)
                {
                    komunikat += (char)Mem.MEMORY[i];//???? nie wiem czy działa
                }
            }
            if (komunikat != "OK")
            {
                System.Console.WriteLine("Blad: zly nadawca. Oczekiwana wartosc to *IN. Otrzymano {0}", nazwa);
                Console.ReadLine();
                Environment.Exit(0);
            }
            i = (int)rejestry.r2 +32;
            if (Mem.MEMORY[i] != '$')
            {
                System.Console.WriteLine("Blad: inna karta. Oczekiwana wartosc to $JOB. Otrzymano {0}{1}{2}{3}", (char)Mem.MEMORY[i], (char)Mem.MEMORY[i+1], (char)Mem.MEMORY[i+2], (char)Mem.MEMORY[i+3]);//domyslnie powinien jeszcze raz czytać
                Console.ReadLine();
                Environment.Exit(0);
            }
            i += 32;//ustawienie na 64 bajt pamieci podr
            rejestry.r2 = i;
            Mem.MEMORY[i++] = (byte)'U';
            Mem.MEMORY[i++] = (byte)'S';
            Mem.MEMORY[i++] = (byte)'E';
            Mem.MEMORY[i++] = (byte)'R';
            Mem.MEMORY[i++] = (byte)'P';
            Mem.MEMORY[i++] = (byte)'R';
            Mem.MEMORY[i++] = (byte)'O';
            Mem.MEMORY[i++] = (byte)'G';



        }

        public static void SCAN()
        {

            //schemat 7.9
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
           
           ibsub1.LAST_PCB_ALL = iplrtn;
           ibsub2.LAST_PCB_ALL = iplrtn;

           ibsub1.NEXT_PCB_GROUP = ibsub1;
           ibsub2.NEXT_PCB_GROUP = ibsub2;

           ibsub1.LAST_PCB_GROUP = ibsub1;
           ibsub2.LAST_PCB_GROUP = ibsub2;


           //Tworzy procesy IBSUB dla każdego strumienia zleceń (2x)
           //Korzysta z interpretera
           
       }
   }
}
