

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;

namespace Memory
{

    public static class Mem
    {
        public enum rozkaz : byte { SVC, MOV, ADD, SUB, MUL, DIV, INC, DEC, JUMPF, JUMPR, JZ, JMP, METHOD, FLAG, POWROT, KONIEC, JUMPV };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, LR, MEM, WART, SEM, PROG };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY, POCZATEK_MEM, KONIEC_MEM };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };



        public static List<FSB> FSB_LIST = new List<FSB>();
        public static SEMAPHORE MEMORY_SEM = new SEMAPHORE();//semafor z domyslna wartoscia 0
        public static SEMAPHORE FSBSEM = new SEMAPHORE(1);//semafor wyłączności dostępu do listy bloków FSB
        public static byte[] MEMORY = new byte[65536];
        public static object r0, r1, r2, r3, r4, r5, r6, r7, r8, r9;
        public static int podreczna;

        public static int zaladujXA(int m)
        {
            int i;
            for (i = 0; i < XA.Length; i++)
            {
                Mem.MEMORY[i + m] = XA[i];
            }
            return m + i + 1;
        }

        public static int zaladujXF(int m)
        {
            int i;
            for (i = 0; i < XF.Length; i++)
            {
                Mem.MEMORY[i + m] = XF[i];
            }
            return m + i + 1;
        }

        public static void PODRECZNA()
        {
            podreczna = 1;
            int best = -1;
            int temp = 65536;
            int temp1 = 256;
            if (temp1 > 65536)
            {
                Console.Write("Rozmiar pamięci mniejszy od rozmiaru procesu!!!");

            }
            else
            {
                for (int i = 0; i != FSB_LIST.Count; i++)
                {
                    if (temp1 <= FSB_LIST[i].wielkosc && FSB_LIST[i].wielkosc - temp1 < temp)
                    {
                        best = FSB_LIST[i].pocz;
                        temp = FSB_LIST[i].wielkosc - temp1;
                    }
                }
            }

            if (best != -1)
            {

                rejestry.r3 = best;
                rejestry.r0 = 0;

            }
            else
            {
                best = 1;
                rejestry.r0 = best;
            }
        }

        public static void PRZESZUKAJ_LISTE()
        {
            int best = -1;
            int temp = 65536;

            int temp1 = Mem.MEMORY[(int)rejestry.r2];
            temp1 = temp1 << 8;
            temp1 += Mem.MEMORY[(int)rejestry.r2 + 1];

            if (temp1 > 65536)
            {
                Console.Write("Rozmiar pamięci mniejszy od rozmiaru procesu!!!");

            }
            else
            {
                for (int i = 0; i != FSB_LIST.Count; i++)
                {
                    if (temp1 <= FSB_LIST[i].wielkosc && FSB_LIST[i].wielkosc - temp1 < temp)
                    {
                        best = FSB_LIST[i].pocz;
                        temp = FSB_LIST[i].wielkosc - temp1;
                    }
                }
            }

            if (best != -1)
            {
                int tmp = best;
                rejestry.r3 = tmp;
                tmp = 0x00001100;
                tmp = tmp >> 8;
                Mem.MEMORY[(int)rejestry.r2 + 2] = (byte)tmp;
                tmp = 0x00000011;
                Mem.MEMORY[(int)rejestry.r2 + 3] = (byte)tmp;
                rejestry.r0 = 0;


            }
            else
            {
                best = 1;
                rejestry.r0 = best;
            }
        }

        public static void WART_MEMORY()
        {
            rejestry.r0 = MEMORY_SEM.VALUE;
        }

        public static void POCZATEK_MEM()
        {
            r0 = rejestry.r0;
            r1 = rejestry.r1;
            r2 = rejestry.r2;
            r3 = rejestry.r3;
            r4 = rejestry.r4;
            r5 = rejestry.r5;
            r6 = rejestry.r6;
            r7 = rejestry.r7;
            r8 = rejestry.r8;
            r9 = rejestry.r9;
            
        }

        public static void KONIEC_MEM()
        {
            rejestry.r0 = r0;
            rejestry.r1 = r1;
            rejestry.r2 = r2;
            if (podreczna != 1)
            {
                rejestry.r3 = r3;
            }
            rejestry.r4 = r4;
            rejestry.r5 = r5;
            rejestry.r6 = r6;
            rejestry.r7 = r7;
            rejestry.r8 = r8;
            rejestry.r9 = r9;
            
        }

        public static byte[] XA = new byte[]
            {
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.POCZATEK_MEM,
                (byte)rozkaz.MOV,(byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//Blokuje dostęp do listy
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R0,28,//Skok w przypadku gdy chodzi tylko o pamięć podręczną
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.PRZESZUKAJ_LISTE,//Przeszukuje liste funkcja w C#
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R0,13,//Jak znalazło wolne bloki to skacze do XB, jak nie znalazło leci dalej
                (byte)rozkaz.MOV, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
                (byte)rozkaz.MOV,(byte)wartosc_TYP.SEM, (byte)wartosc_SEM.MEMORY,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//dodanie procesu do oczekujacych, semafor MEMORY
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.WART,35,//Jeżeli zrobiło P na semaforze MEMORY to skacze do POWROT
                (byte)rozkaz.SVC,(byte)wartosc_SVC.B, //Wywołuje XB
                (byte)rozkaz.MOV, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.WART,25,//Skok do POWROT jeżeli normalnie przydzielono blok
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.PODRECZNA,
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R0,13,//Jak znalazło wolne bloki to skacze do XB, jak nie znalazło leci dalej
                (byte)rozkaz.MOV, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
                (byte)rozkaz.MOV,(byte)wartosc_TYP.SEM, (byte)wartosc_SEM.MEMORY,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//dodanie procesu do oczekujacych, semafor MEMORY
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.WART,7,//Jeżeli zrobiło P na semaforze MEMORY to skacze do POWROT
                (byte)rozkaz.SVC,(byte)wartosc_SVC.B,
                (byte)rozkaz.MOV, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.KONIEC_MEM,
                (byte)rozkaz.POWROT,
     
            };

        public static byte[] XF = new byte[]
            {
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.POCZATEK_MEM,
                (byte)rozkaz.MOV, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//Blokuje dostęp do listy
                (byte)rozkaz.SVC,(byte)wartosc_SVC.B, //Wywołuje XB
                (byte)rozkaz.MOV, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.V,
                (byte)rozkaz.MOV,(byte)wartosc_TYP.SEM, (byte)wartosc_SEM.MEMORY,
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.WART_MEMORY,
                (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R0,7,
                (byte)rozkaz.SVC,(byte)wartosc_SVC.V,
                (byte)rozkaz.INC,(byte)wartosc_TYP.R0,
                (byte)rozkaz.JUMPV,(byte)wartosc_TYP.R0,7,//ma skoczyć do tyłu i znowu wykonać V na semaforze MEMORY
                (byte)rozkaz.METHOD,(byte)wartosc_METHOD.KONIEC_MEM,
                (byte)rozkaz.POWROT,
     
            };
        public static void XB()
        {
            int r = 0;
            if (rejestry.r2.GetType() == typeof(int))
            {
                r = Mem.MEMORY[(int)rejestry.r2];
                r = r << 8;
                r += Mem.MEMORY[(int)rejestry.r2 + 1];
            }

            if (podreczna == 1)
                r = 256;

            int a = (int)rejestry.r3;


            FSB fsb = new FSB(a + r, 65535, 65535 - (a + r - 1));

            if (FSB_LIST.Count == 1)
            {
                FSB_LIST.Clear();
                FSB_LIST.Add(fsb);
            }
            else
            {
                int tmp = 0;
                int tmp1 = 0;
                for (int i = 0; i < FSB_LIST.Count; i++)
                {
                    if (FSB_LIST[i].pocz == a)
                    {
                        tmp = FSB_LIST[i].koniec;
                        FSB_LIST.RemoveAt(i);
                        if (a + r == FSB_LIST[i].koniec)
                            tmp1 = 1;
                    }
                }


                if (tmp1 == 0)
                {
                    fsb = new FSB(a + r, tmp, tmp - (a + r - 1));
                    FSB_LIST.Add(fsb);
                }

                if (tmp1 == 0)
                {
                    for (int i = 0; i < FSB_LIST.Count; i++)
                    {
                        if (FSB_LIST[i].koniec == FSB_LIST[i + 1].pocz - 1)
                        {
                            int pocz, konie, roz;
                            pocz = FSB_LIST[i].pocz;
                            konie = FSB_LIST[i + 1].koniec;
                            roz = FSB_LIST[i].wielkosc + FSB_LIST[i + 1].wielkosc;

                            FSB_LIST.RemoveAt(i);
                            FSB_LIST.RemoveAt(i + 1);

                            fsb = new FSB(pocz, konie, roz);
                            FSB_LIST.Add(fsb);
                        }
                    }

                }
                var sortedList = FSB_LIST.OrderBy(x => x.wielkosc).ToList();
                FSB_LIST = sortedList;
            }

        }


        public static bool start(int i)
        {
            FSB free = new FSB(i, 65535, 65536 - i);
            FSB_LIST.Add(free);
            return true;
        }
    }

    public class FSB
    {
        public int pocz;
        public int koniec;
        public int wielkosc;

        public FSB(int p, int k, int w)
        {
            pocz = p;
            koniec = k;
            wielkosc = w;
        }

    }
}

