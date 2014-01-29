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
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };


        public static List<FSB> FSB_LIST = new List<FSB>();
        public static SEMAPHORE MEMORY_SEM = new SEMAPHORE();//semafor z domyslna wartoscia 0
        public static SEMAPHORE FSBSEM = new SEMAPHORE(1);//semafor wyłączności dostępu do listy bloków FSB
        public static byte[] MEMORY = new byte[65536];

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
                rejestry.r2 = 0;
 
            }
            else
            {
                best = 1;
                rejestry.r2 = best;
            }
        }
 
        public static void PRZESZUKAJ_LISTE()
        {
            int best = -1;
            int temp = 65536;
 
            int temp1 = Mem.MEMORY[(int)rejestry.r1];
            temp1 = temp1 << 8;
            temp1 += Mem.MEMORY[(int)rejestry.r1 + 1];
 
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
                tmp = 0x00001100;
                tmp = tmp >> 8;
                Mem.MEMORY[(int)rejestry.lr + 2] = (byte)tmp;
                tmp = 0x00000011;
                Mem.MEMORY[(int)rejestry.lr + 3] = (byte)tmp;
                rejestry.r2 = 0;
 
 
            }
            else
            {
                best = 1;
                rejestry.r2 = best;
            }
        }
 
        public static void WART_MEMORY()
        {
            rejestry.r0 = MEMORY_SEM.VALUE;
        }
 
        public static byte[] XA = new byte[]
        {
 
            (byte)rozkaz.MOV,(byte)wartosc_TYP.R1,(byte)wartosc_TYP.R2,  //Zapisuje do R1 adres pamięci(MEM) od której zaczyna się 5bajtów (rozmiar,adres,pading)
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//Blokuje dostęp do listy
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R9,28,//Skok w przypadku gdy chodzi tylko o pamięć podręczną
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.PRZESZUKAJ_LISTE,//Przeszukuje liste funkcja w C#
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R2,14,//Jak znalazło wolne bloki to skacze do XB, jak nie znalazło leci dalej
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.MEMORY,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//dodanie procesu do oczekujacych, semafor MEMORY
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.WART,30,//Jeżeli zrobiło P na semaforze MEMORY to skacze do POWROT
            (byte)rozkaz.SVC,(byte)wartosc_SVC.B, //Wywołuje XB
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.WART,20,//Skok jeżeli normalnie przydzielono blok
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.PODRECZNA,
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R2,14,//Jak znalazło wolne bloki to skacze do XB, jak nie znalazło leci dalej
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.V,//Odblokowuje semafor FSBSEM
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.MEMORY,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//dodanie procesu do oczekujacych, semafor MEMORY
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.WART,2,//Jeżeli zrobiło P na semaforze MEMORY to skacze do POWROT
            (byte)rozkaz.SVC,(byte)wartosc_SVC.B,
            (byte)rozkaz.POWROT,
 
        };
 
        public static byte[] XF = new byte[]
        {
            (byte)rozkaz.MOV,(byte)wartosc_TYP.R1,(byte)wartosc_TYP.R2,  //Zapisuje do R1 adres pamięci(MEM) od której zaczyna się 5bajtów (rozmiar,adres,pading)
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.P,//Blokuje dostęp do listy
            (byte)rozkaz.SVC,(byte)wartosc_SVC.B, //Wywołuje XB
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.FSBSEM,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.V,
            (byte)rozkaz.MOV, (byte)wartosc_TYP.R2, (byte)wartosc_TYP.SEM, (byte)wartosc_SEM.MEMORY,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.WART_MEMORY,
            (byte)rozkaz.JUMPF,(byte)wartosc_TYP.R0,6,
            (byte)rozkaz.SVC,(byte)wartosc_SVC.V,
            (byte)rozkaz.INC,(byte)wartosc_TYP.R0,
            (byte)rozkaz.JUMPV,(byte)wartosc_TYP.R0,6,//ma skoczyć do tyłu i znowu wykonać V na semaforze MEMORY
            (byte)rozkaz.POWROT,
 
        };
        public static void XB()
        {
            int r = Mem.MEMORY[(int)rejestry.r1];
            r = r << 8;
            r += Mem.MEMORY[(int)rejestry.r1 + 1];
 
            int a = Mem.MEMORY[(int)rejestry.r1 + 2];
            a = a << 8;
            a += Mem.MEMORY[(int)rejestry.r1 + 3];
 
 
            FSB fsb = new FSB(a + r + 1, 65536, 65536);
 
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
                        tmp1 = FSB_LIST[i].wielkosc;
                        FSB_LIST.RemoveAt(i);
                    }
                }
 
                fsb = new FSB(a + r + 1, tmp, tmp1);
                FSB_LIST.Add(fsb);
 
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

