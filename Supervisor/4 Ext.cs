

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
        public static PCB wersja;


    }

    class Ext
    {

        public static int il_danych, adres;
        public static int linia = 1;
        public static string com;
        public enum rozkaz : byte { SVC, MOV, ADD, SUB, MUL, DIV, INC, DEC, JUMPF, JUMPR, JZ, JMP, METHOD, FLAG, POWROT, KONIEC, JUMPV };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, LR, MEM, WART, SEM, PROG };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY, POCZATEK_MEM, KONIEC_MEM, GRUPA };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };



        private static byte[] mem = new byte[]{


                        (byte)rozkaz.SVC,       (byte)wartosc_SVC.E, 
                        //zapisywanie grupy procesów
                        (byte)wartosc_METHOD.GRUPA,
           
                        //Przydział pamięci roboczej  
                         
     
                        //ustawienie flagi
                        (byte)rozkaz.FLAG, 10,


                        (byte)rozkaz.SVC,       (byte)wartosc_SVC.R,
                        (byte)wartosc_METHOD.SPRAWDZENIE,
     
                        (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,
                       
                        (byte)rozkaz.JMP, 10,
                        };


        public static void CZYTNIK(string com, int xxxx, int ilosc)
        {
            UCB device = new UCB();
            if (com == "READ")
            {
                if (zawiadowca.RUNNING.LAST_PCB_GROUP == UCB.wersja)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader("plik1.txt");
                    string JOB = file.ReadLine();
                    if (JOB[0] == '$' && JOB[1] == 'J' && JOB[2] == 'O' && JOB[3] == 'B')
                    {
                        //zapisanie wszystkich lini do stringów
                        string[] lines = System.IO.File.ReadAllLines("plik1.txt");

                        //zapisanie wyrazów z jednej lini do stringów
                        string[] wyrazy = lines[linia].Split(' ');

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
                        int counter = 0;
                        foreach (string typeString in wyrazy)
                        {
                            rozkaz rozkazValue;
                            if (Enum.TryParse(typeString, out rozkazValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)rozkazValue;
                            }


                            wartosc_SVC wartosc_SVCValue;
                            if (Enum.TryParse(typeString, out wartosc_SVCValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)wartosc_SVCValue;
                            }

                            wartosc_TYP wartosc_TYPValue;
                            if (Enum.TryParse(typeString, out wartosc_TYPValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)wartosc_TYPValue;
                            }

                            wartosc_SEM wartosc_SEMValue;
                            if (Enum.TryParse(typeString, out wartosc_SEMValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)wartosc_SEMValue;
                            }

                            counter++;
                        }



                        linia++;
                    }
                    else
                    {
                        System.Console.WriteLine("Brak $JOB'a!");
                        Console.Read();
                        Environment.Exit(0);
                    }


                }


                if (zawiadowca.RUNNING.LAST_PCB_GROUP != UCB.wersja)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader("plik2.txt");
                    string JOB = file.ReadLine();
                    if (JOB[0] == '$' && JOB[1] == 'J' && JOB[2] == 'O' && JOB[3] == 'B')
                    {
                        //zapisanie wszystkich lini do stringów
                        string[] lines = System.IO.File.ReadAllLines("plik2.txt");

                        //zapisanie wyrazów z jednej lini do stringów
                        string[] wyrazy = lines[linia].Split(' ');

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
                        int counter = 0;
                        foreach (string typeString in wyrazy)
                        {
                            rozkaz rozkazValue;
                            if (Enum.TryParse(typeString, out rozkazValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)rozkazValue;
                            }


                            wartosc_SVC wartosc_SVCValue;
                            if (Enum.TryParse(typeString, out wartosc_SVCValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)wartosc_SVCValue;
                            }

                            wartosc_TYP wartosc_TYPValue;
                            if (Enum.TryParse(typeString, out wartosc_TYPValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)wartosc_TYPValue;
                            }

                            wartosc_SEM wartosc_SEMValue;
                            if (Enum.TryParse(typeString, out wartosc_SEMValue))
                            {
                                Mem.MEMORY[xxxx + counter + 2] = (byte)wartosc_SEMValue;
                            }

                            counter++;
                        }



                        linia++;
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
                if (zawiadowca.RUNNING.LAST_PCB_GROUP == UCB.wersja)
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

                if (zawiadowca.RUNNING.LAST_PCB_GROUP != UCB.wersja)
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
            if (UCB.wersja != null) UCB.wersja = zawiadowca.RUNNING.LAST_PCB_GROUP;
        }


    }
}

