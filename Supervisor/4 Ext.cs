

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




    class Ext
    {
        public static PCB wersja;
        public static int il_danych, adres;
        public static int linia1 = 0;
        public static int linia2 = 0;
        public static string com;
        public enum rozkaz : byte { SVC, MOV, ADD, SUB, MUL, DIV, INC, DEC, JUMPF, JUMPR, JZ, JMP, METHOD, FLAG, POWROT, KONIEC, JUMPV };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, LR, MEM, WART, SEM, PROG };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY, POCZATEK_MEM, KONIEC_MEM, GRUPA, ZERUJ_PAM, XA, XF, XD, XR, XS };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };


        private static byte[] mem = new byte[]{
     
                         //Przydział pamięci roboczej  
                        (byte)rozkaz.SVC,       (byte)wartosc_SVC.E,
     
                        //zapisywanie grupy procesów
                        (byte)rozkaz.METHOD, (byte)wartosc_METHOD.GRUPA,
           
     
                        //ustawienie flagi
                        (byte)rozkaz.FLAG, 10,
     
                        //pobranie komunikatu
                        (byte)rozkaz.SVC,       (byte)wartosc_SVC.R,
     
                        (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.SPRAWDZENIE,
     
                        //wysłanie komunikatu
                        (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,
                       
                        (byte)rozkaz.JMP, (byte)wartosc_TYP.WART, 10
                       
                       
         
                   
                     
     
           
     
            /*,"","","","",""*/};


        public static void CZYTNIK(string com, int xxxx, int ilosc)
        {
            Console.WriteLine("Uruchomienie czytnika.");
            if (com == "READ")
            {
                if (zawiadowca.RUNNING.LAST_PCB_GROUP == Ext.wersja)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader("plik1.txt");
                    string JOB = file.ReadLine();
                    Console.WriteLine("Odczytanie $JOB z pliku1.");
                    if (JOB[0] == '$' && JOB[1] == 'J' && JOB[2] == 'O' && JOB[3] == 'B')
                    {
                        if (linia1 == 0)
                        {
                            for (int i = 0; i < JOB.Length; i++)
                            {
                                Mem.MEMORY[xxxx + i] = Convert.ToByte(JOB[i]);
                            }
                            linia1++;
                        }
                        else
                        {
                            //zapisanie wszystkich lini do stringów
                            string[] lines = System.IO.File.ReadAllLines("plik1.txt");

                            //komunikat NO
                            if (lines.Length < linia1)
                            {
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 8] = 2;
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 9] = Convert.ToByte('N');
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 10] = Convert.ToByte('O');
                            }
                            else
                            {

                                //zapisanie wyrazów z jednej lini do stringów
                                string[] wyrazy = lines[linia1].Split(' ');

                                //przesłanie długości do dwóch pierwszych bajtów
                                byte[] tmpB = BitConverter.GetBytes(wyrazy.Length);
                                if (BitConverter.IsLittleEndian == true)
                                {
                                    Mem.MEMORY[xxxx] = tmpB[1];
                                    Mem.MEMORY[xxxx + 1] = tmpB[0];
                                }
                                else
                                {
                                    Mem.MEMORY[xxxx] = tmpB[2];
                                    Mem.MEMORY[xxxx + 1] = tmpB[3];
                                }


                                int counter1 = 0;
                                foreach (string typeString in wyrazy)
                                {
                                    rozkaz rozkazValue;
                                    if (Enum.TryParse(typeString, out rozkazValue))
                                    {
                                        Mem.MEMORY[xxxx + counter1 + 2] = (byte)rozkazValue;
                                    }


                                    wartosc_SVC wartosc_SVCValue;
                                    if (Enum.TryParse(typeString, out wartosc_SVCValue))
                                    {
                                        Mem.MEMORY[xxxx + counter1 + 2] = (byte)wartosc_SVCValue;
                                    }

                                    wartosc_TYP wartosc_TYPValue;
                                    if (Enum.TryParse(typeString, out wartosc_TYPValue))
                                    {
                                        Mem.MEMORY[xxxx + counter1 + 2] = (byte)wartosc_TYPValue;
                                    }

                                    wartosc_SEM wartosc_SEMValue;
                                    if (Enum.TryParse(typeString, out wartosc_SEMValue))
                                    {
                                        Mem.MEMORY[xxxx + counter1 + 2] = (byte)wartosc_SEMValue;
                                    }

                                    counter1++;
                                }


                                //przesłanie komunikatu OK
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 8] = 2;
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 9] = Convert.ToByte('O');
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 10] = Convert.ToByte('K');

                                linia1++;
                            }
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Brak $JOB'a!");
                        Console.Read();
                        Environment.Exit(0);
                    }


                }


                else if (zawiadowca.RUNNING.LAST_PCB_GROUP != Ext.wersja)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader("plik2.txt");
                    string JOB = file.ReadLine();
                    Console.WriteLine("Odczytanie $JOB z pliku2.");
                    if (JOB[0] == '$' && JOB[1] == 'J' && JOB[2] == 'O' && JOB[3] == 'B')
                    {
                        if (linia2 == 0)
                        {
                            for (int i = 0; i < JOB.Length; i++)
                            {
                                Mem.MEMORY[xxxx + i] = Convert.ToByte(JOB[i]);
                            }
                            linia2++;
                        }
                        else
                        {
                            //zapisanie wszystkich lini do stringów
                            string[] lines = System.IO.File.ReadAllLines("plik2.txt");

                            //komunikat NO
                            if (lines.Length < linia2)
                            {
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 8] = 2;
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 9] = Convert.ToByte('N');
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 10] = Convert.ToByte('O');
                            }
                            else
                            {

                                //zapisanie wyrazów z jednej lini do stringów
                                string[] wyrazy = lines[linia2].Split(' ');

                                //przesłanie długości do dwóch pierwszych bajtów
                                byte[] tmpB = BitConverter.GetBytes(wyrazy.Length);
                                if (BitConverter.IsLittleEndian == true)
                                {
                                    Mem.MEMORY[xxxx] = tmpB[1];
                                    Mem.MEMORY[xxxx + 1] = tmpB[0];
                                }
                                else
                                {
                                    Mem.MEMORY[xxxx] = tmpB[2];
                                    Mem.MEMORY[xxxx + 1] = tmpB[3];
                                }

                                //dla każdego wyrazu enum, wyrazy.legth - przesłanie do dwóch pierwszych bytów,
                                int counter2 = 0;
                                foreach (string typeString in wyrazy)
                                {
                                    rozkaz rozkazValue;
                                    if (Enum.TryParse(typeString, out rozkazValue))
                                    {
                                        Mem.MEMORY[xxxx + counter2 + 2] = (byte)rozkazValue;
                                    }


                                    wartosc_SVC wartosc_SVCValue;
                                    if (Enum.TryParse(typeString, out wartosc_SVCValue))
                                    {
                                        Mem.MEMORY[xxxx + counter2 + 2] = (byte)wartosc_SVCValue;
                                    }

                                    wartosc_TYP wartosc_TYPValue;
                                    if (Enum.TryParse(typeString, out wartosc_TYPValue))
                                    {
                                        Mem.MEMORY[xxxx + counter2 + 2] = (byte)wartosc_TYPValue;
                                    }

                                    wartosc_SEM wartosc_SEMValue;
                                    if (Enum.TryParse(typeString, out wartosc_SEMValue))
                                    {
                                        Mem.MEMORY[xxxx + counter2 + 2] = (byte)wartosc_SEMValue;
                                    }

                                    counter2++;
                                }

                                //przesłanie komunikatu OK
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 8] = 2;
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 9] = Convert.ToByte('O');
                                Mem.MEMORY[(int)wartosc_TYP.R2 + 10] = Convert.ToByte('K');

                                linia2++;
                            }
                        }
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
                Console.WriteLine("Uruchomienie drukarki.");
                if (zawiadowca.RUNNING.LAST_PCB_GROUP == Ext.wersja)
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

                if (zawiadowca.RUNNING.LAST_PCB_GROUP != Ext.wersja)
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
            int pocz = (int)rejestry.r2 + 9;
            string c = null;
            UTF8Encoding kodowanie = new UTF8Encoding();
            c = kodowanie.GetString(Mem.MEMORY, pocz, 4);
            if (c != "READ" && c != "PRIN")
            {
                System.Console.WriteLine("Zły komunikat!");
                Console.Read();
                Environment.Exit(0);
            }
            else
            {
                Ext.com = c;
                pocz += 4;
                int il_danych = Mem.MEMORY[pocz];
                int il_danych1 = il_danych << 8;
                il_danych1 += Mem.MEMORY[pocz + 1];
                Ext.il_danych = il_danych1;

                int adres = Mem.MEMORY[pocz + 2];
                int adres1 = adres << 8;
                adres1 += Mem.MEMORY[pocz + 3];
                Ext.adres = adres1;

            }

            CZYTNIK(Ext.com, Ext.adres, Ext.il_danych);
        }

        public static int zaladuj(int m)
        {
            int i;
            for (i = 0; i < mem.Length; i++)
            {
                Mem.MEMORY[i + m] = mem[i];
            }
            return m + i + 1;
        }

        public static void GRUPA()
        {
            if (Ext.wersja != null) Ext.wersja = zawiadowca.RUNNING.LAST_PCB_GROUP;
            rejestry.r2 = rejestry.r3;
        }


    }
}

