using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Processor;
//do dokończenia
namespace Interpreter
{
    public static class Inter
    {
        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, METHOD, CREATE };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_CREATE : byte { KOM, PCB };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART};
        public enum wartosc_SEM : byte { MEMORY };


        public static void Run()
        {
            if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.SVC)
            {
                rejestry.lr++;

                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.P)
                {
                    //wywołaj metode P klasy semafor
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.V)
                {
                    //wywołaj metode V klasy semafor
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.G)
                {
                    //wywołaj metode Run zawiadowcy
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.A)
                {
                    //wywołaj metode A klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.E)
                {
                    //wywołaj metode E klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.F)
                {
                    //wywołaj metode F klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.B)
                {
                    //wywołaj metode B klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.C)
                {
                    //wywołaj metode C klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.D)
                {
                    //wywołaj metode D klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.H)
                {
                    //wywołaj metode H klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.I)
                {
                    //wywołaj metode I klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.J)
                {
                    //wywołaj metode J klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.N)
                {
                    //wywołaj metode N klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.R)
                {
                    //wywołaj metode R klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.S)
                {
                    //wywołaj metode S klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.Y)
                {
                    //wywołaj metode Y klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.Z)
                {
                    //wywołaj metode Z klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.Q)
                {
                    //wywołaj metode Q klasy Proc
                }
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.ADD)
            {
                rejestry.lr++;
                //dokończyć
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.MOV)
            {
                rejestry.lr++;
                //dokończyć
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.DIV)
            {
                rejestry.lr++;
                //dokończyć
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.SUB)
            {
                rejestry.lr++;
                //dokończyć
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.INC)
            {
                rejestry.lr++;
                //dokończyć
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.METHOD)
            {
                rejestry.lr++;
                //dokończyć
            }

        }
    }

    //wywalićVVVVVVVV
    public class CALL
    {
        public int rozkaz;
        public int wartosc1;
        public int wartosc2;
        public int wartosc3;
        public int wartosc4;
        public int wartosc5;
        public string napis1;

        //Przeciążone konstruktory (-2000 to wartość dla nie określanego pola, przesyłanie wartości odbywa się przez ENUM, bądź dosłownie np stringiem)
        CALL()
        {
            rozkaz = -2000;
            wartosc1 = -2000;
            wartosc2 = -2000;
            wartosc3 = -2000;
            wartosc4 = -2000;
            wartosc5 = -2000;
            napis1 = null;
        }
        CALL(string s)
        {
            rozkaz = -2000;
            wartosc1 = -2000;
            wartosc2 = -2000;
            wartosc3 = -2000;
            wartosc4 = -2000;
            wartosc5 = -2000;
            napis1 = s;
        }
        CALL(int r)
        {
            rozkaz = r;
            wartosc1 = -2000;
            wartosc2 = -2000;
            wartosc3 = -2000;
            wartosc4 = -2000;
            wartosc5 = -2000;
        }
        CALL(int r, int w1)
        {
            rozkaz = r;
            wartosc1 = w1;
            wartosc2 = -2000;
            wartosc3 = -2000;
            wartosc4 = -2000;
            wartosc5 = -2000;
        }
        CALL(int r, int w1, int w2)
        {
            rozkaz = r;
            wartosc1 = w1;
            wartosc2 = w2;
            wartosc3 = -2000;
            wartosc4 = -2000;
            wartosc5 = -2000;
        }
        CALL(int r, int w1, int w2, int w3)
        {
            rozkaz = r;
            wartosc1 = w1;
            wartosc2 = w2;
            wartosc3 = w3;
            wartosc4 = -2000;
            wartosc5 = -2000;
        }
        CALL(int r, int w1, int w2, int w3, int w4)
        {
            rozkaz = r;
            wartosc1 = w1;
            wartosc2 = w2;
            wartosc3 = w3;
            wartosc4 = w4;
            wartosc5 = -2000;
        }
        CALL(int r, int w1, int w2, int w3, int w4, int w5)
        {
            rozkaz = r;
            wartosc1 = w1;
            wartosc2 = w2;
            wartosc3 = w3;
            wartosc4 = w4;
            wartosc5 = w5;
        }
    }
}
