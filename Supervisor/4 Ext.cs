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
        bool adres;
    }

    class Ext
    {

        public static int il_danych, adres;
        public static string com;
        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, JUMPF, JUMPR, JUMP, METHOD, FLAG };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART, SEM };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN };

        

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
                   // (byte)rozkaz.MOV,       (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.USER,
                   // (byte)rozkaz.SVC,       (byte)wartosc_SVC.P,
                   
                    (byte)wartosc_METHOD.CZYTNIK,    //dopisać przekazanie wartości - com, adres, il_danych
                   
 
 
 
               
                    (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.SPRAWDZENIE,
                    (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.CZYTNIK,
 
 
       
 
        /*,"","","","",""*/};


        public static void CZYTNIK(string com, int xxxx, int ilosc)
        {
            UCB device = new UCB();
            //zawiadowca.RUNNING
            if (com == "READ")
            {
                if (zawiadowca.RUNNING.adres_pocz == 512)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader("plik1.txt");
                    string JOB = file.ReadLine();
                    if (JOB == "$JOB")
                    {
                        byte[] array = Encoding.ASCII.GetBytes(JOB);
                        Mem.MEMORY[xxxx] = array[0];
                        Mem.MEMORY[xxxx + 1] = array[1];
                        Mem.MEMORY[xxxx + 2] = array[2];
                        Mem.MEMORY[xxxx + 3] = array[3];
                    }
                    else
                    {
                        System.Console.WriteLine("Brak $JOB'a!");
                        Console.Read();
                        Environment.Exit(0);
                    }

                    string[] lines = System.IO.File.ReadAllLines("plik1.txt");
                    string[][] words;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //words[i][]=lines[i].Split(' ');
                    }
                }


                if (zawiadowca.RUNNING.adres_pocz == 1536)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader("plik2.txt");
                    string JOB = file.ReadLine();
                    if (JOB == "$JOB")
                    {
                        byte[] array = Encoding.ASCII.GetBytes(JOB);
                        Mem.MEMORY[xxxx] = array[0];
                        Mem.MEMORY[xxxx + 1] = array[1];
                        Mem.MEMORY[xxxx + 2] = array[2];
                        Mem.MEMORY[xxxx + 3] = array[3];
                    }
                    else
                    {
                        System.Console.WriteLine("Brak $JOB'a!");
                        Console.Read();
                        Environment.Exit(0);
                    }
                }

            }


            if (com == "PRIN")
            {
                if (zawiadowca.RUNNING.adres_pocz == 512)
                {
                    byte[] array = new byte[ilosc];
                    for (; xxxx < xxxx + ilosc; xxxx++)
                    {
                        int i = 0;
                        array[i] = Mem.MEMORY[xxxx];
                        i++;
                    }

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("drukarka1.txt", true))
                    {
                        for (int f = 0; f < ilosc; f++)
                        {
                            file.Write(array[f]);
                            file.Write(" ");
                        }
                    }
                }

                if (zawiadowca.RUNNING.adres_pocz == 1536)
                {
                    byte[] array = new byte[ilosc];
                    for (; xxxx < xxxx + ilosc; xxxx++)
                    {
                        int i = 0;
                        array[i] = Mem.MEMORY[xxxx];
                        i++;
                    }

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("drukarka2.txt", true))
                    {
                        for (int f = 0; f < ilosc; f++)
                        {
                            file.Write(array[f]);
                            file.Write(" ");
                        }
                    }
                }

            }






        }



        public static void SPRAWDZENIE()
        {

            int pocz = Mem.MEMORY[(int)wartosc_TYP.R2 + 9];
            string c = "";
            for (; pocz < pocz + 4; pocz++)
            {
                c += Mem.MEMORY[pocz].ToString();
            }
            if (c != "READ" && c != "PRIN")
            {
                System.Console.WriteLine("Zły komunikat!");
                Console.Read();
                Environment.Exit(0);
            }
            else
            {
                Ext.com = c;

                int il_danych = Mem.MEMORY[pocz + 1];
                int il_danych1 = il_danych << 8;
                il_danych1 += Mem.MEMORY[pocz + 2];
                Ext.il_danych = il_danych1;

                int adres = Mem.MEMORY[pocz + 3];
                int adres1 = adres << 8;
                adres1 += Mem.MEMORY[pocz + 4];
                Ext.adres = adres1;

            }

        }

        public static void zaladuj(int m)
        {
            for (int i = 0; i < mem.Length; i++)
            {
                Mem.MEMORY[i + m] = mem[i];
            }
        }


    }
}