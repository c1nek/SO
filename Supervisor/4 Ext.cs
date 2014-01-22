using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;
using Supervisor;
using Memory;

namespace External
{



    public class UCB
    {

        public string adres;
        SEMAPHORE USER;
        SEMAPHORE WAIT;

    }

    class Ext
    {

        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, JUMPF, JUMPR, METHOD, CREATE };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_CREATE : byte { KOM, PCB };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART, SEM };
        public enum wartosc_SEM : byte { MEMORY, USER, WAIT, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, SPRAWDZENIE, CZYTNIK };

        private static byte[] mem = new byte[]{
        //Przydział pamięci roboczej
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.LR,
                    (byte)rozkaz.ADD,       (byte)wartosc_TYP.WART,     11,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.R0,
                    (byte)rozkaz.JUMPF,     (byte)wartosc_TYP.WART,     7,
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.A,         1,0,0,0,4,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.R2,
                    (byte)rozkaz.ADD,       (byte)wartosc_TYP.WART,     2,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R0,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R3,       (byte)wartosc_TYP.MEM,
               
                   
                    ////////////////////////////OBSŁUGA CZYTNIKA\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    (byte)wartosc_METHOD.SPRAWDZENIE,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.USER,
                   
                   
 
 
 
               
                    (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.SPRAWDZENIE,
                    (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.CZYTNIK,
 
 
       
 
        /*,"","","","",""*/};


        public static void CZYTNIK(string com, int xxxx)
         {
             string adres=null;
             UCB device = new UCB();
             if (com == "READ")
             {
                 if (zawiadowca.RUNNING.adres_pocz == 512)
                 {
                     adres = "plik1.txt";
                     device.adres = adres;
                     System.IO.StreamReader file = new System.IO.StreamReader("plik1.txt");
                     string JOB = file.ReadLine();
                     if(JOB == "$JOB")
                     {
                         byte[] array = Encoding.ASCII.GetBytes(JOB);
                         Mem.MEMORY[xxxx]=array[0];
                         Mem.MEMORY[xxxx+1]=array[1];
                         Mem.MEMORY[xxxx+2]=array[2];
                         Mem.MEMORY[xxxx+3]=array[3];
                     }
 
                 }
                 
                 
                 if (zawiadowca.RUNNING.adres_pocz == 1536)
                 {
                 adres = "plik2.txt";
                 device.adres = adres;
                 System.IO.StreamReader file = new System.IO.StreamReader("plik2.txt");
                     string JOB = file.ReadLine();
                     if(JOB == "$JOB")
                     {
                         byte[] array = Encoding.ASCII.GetBytes(JOB);
                         Mem.MEMORY[xxxx]=array[0];
                         Mem.MEMORY[xxxx+1]=array[1];
                         Mem.MEMORY[xxxx+2]=array[2];
                         Mem.MEMORY[xxxx+3]=array[3];
                     }
                 }
 
             }
           
 
                     if (com == "PRIN")
                     {
                         if (zawiadowca.RUNNING.adres_pocz == 512) adres = "drukarka1.txt";
                         if (zawiadowca.RUNNING.adres_pocz == 1536) adres = "drukarka2.txt";
                         device.adres = adres;
                         using (System.IO.StreamWriter file = new System.IO.StreamWriter(adres, true))
                         {
                             //file.WriteLine(string);
                         }
 
                     }
 
                   
 
 
                 
 
         }



        public static void SPRAWDZENIE()
        {
            //int dlugosc = Mem.MEMORY[(int)wartosc_TYP.R2 + 8];
            int pocz = Mem.MEMORY[(int)wartosc_TYP.R2 + 9];
            int temp = pocz;
            string c = "";
            for (; temp < temp + 4; temp++)
            {
                c += Mem.MEMORY[temp].ToString();
            }
            if (c != "READ" && c != "PRIN")
            {
                System.Console.WriteLine("Zły komunikat!");
                string input = Console.ReadLine();
            }
        }


    }
}