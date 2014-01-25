﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;
using Processor;

namespace Memory
{
    
    public static class Mem
    {
        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, JUMPF, JUMPR, JUMP, METHOD, FLAG, POWROT };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART, SEM };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA };
        public enum Eprog : byte { IBSUB, IN, OUT, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };



        public static FSB FSBPTR=null;
        public static SEMAPHORE MEMORY_SEM=new SEMAPHORE();//semafor z domyslna wartoscia 0
        public static SEMAPHORE FSBSEM = new SEMAPHORE(1);//semafor wyłączności dostępu do listy bloków FSB
        public static byte[] MEMORY = new byte[65536];

        public static byte[] XA=new byte[]
        {
            
        };

        public static byte[] XF = new byte[]
        {
            
        };
        public static void XB()
        {

        }

        public static void PRZESZUKAJ_LISTE()
        {
 
        }

        public static void PODRECZNA()
        {
 
        }

        public static bool start(int i)
        {
            //create FSB blocks (called only once by IPLRTN)
            return true;
        }
    }

    public class FSB
    {
        public FSB NEXT;
        public int pocz;
        public int koniec;
        public int wielkosc;

    }
    

}
