using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Processor;
using Supervisor;
using External;
using Process;
//do dokończenia
namespace Interpreter
{
    public static class Inter
    {
        public enum rozkaz : byte { SVC, MOV, ADD, SUB, MUL, DIV, INC, DEC, JUMPF, JUMPR, JZ, JMP, METHOD, FLAG, POWROT, KONIEC, JUMPV };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, LR, MEM, WART, SEM, PROG};
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY, POCZATEK_MEM, KONIEC_MEM, GRUPA, ZERUJ_PAM, XA, XF };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };


        public static byte[] LFlag = new byte[100];//tablica adresów flag

        private static Stack<int> stos = new Stack<int>();//stos wywołań programów

        private static void CWrite(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }//kolorowanie tekstu

        //(MOV, R0|R1|R2|R3|LR, R0|R1|R2|R3|LR) kopiuje wartośc do pierwszego rejestru z drugiego (MOV, pierwszy_rejestr, drugi_rejestr)
        //(MOV, R0|R1|R2|R3|LR, WART, <starszy bajt>, <młodszy bajt>) kopiuje wartość do rejestrów określoną po WART, (rejestry są dwubajtowe)
        //(MOV, MEM, R0|R1|R2|R3|LR) kopiuje wartość rejestru do dwóch następujących po sobie komórek pamięci , adres pierwszej komórki w R1
        //(MOV, MEM, WART, <wartosć>) kopiuje do komórki pamięci wartość po WART, adres komórki w R1
        //(MOV, SEM, MEMORY|COMMON|RECEIVER|R2_COMMON|R2_RECEIVER|FSBSEM) Ustaiwa referencję na semafor w rejestrze 2, R2_* ustawia referencje na semafor w rejestrze 2, z bloku pcb wskazywanego przez r2

        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void Run()
        {

            CWrite(ConsoleColor.Green, "Interpreter: ");

            
            
            if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.SVC)
            {
                rejestry.lr++;

                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.P)
                {
                    //wywołaj metode P klasy semafor
                    rejestry.lr++;
                    Console.Write("SVC P");
                    SEMAPHORE.P();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.V)
                {
                    rejestry.lr++;
                    Console.Write("SVC V");
                    SEMAPHORE.V();
                    //wywołaj metode V klasy semafor
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.G)
                {
                    rejestry.lr++;
                    Console.Write("SVC .");
                    return;
                    //wywołaj metode Run zawiadowcy
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.A)
                {
                    rejestry.lr++;
                    Console.Write("SVC A");
                    rejestry.r0 = 1;
                    stos.Push(rejestry.lr);
                    rejestry.lr = IPLRTN.adrProg[(int)Eprog.A];
                
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.E)
                {
                    rejestry.lr++;
                    Console.Write("SVC E");
                    rejestry.r0 = 0;
                    stos.Push(rejestry.lr);
                    rejestry.lr = IPLRTN.adrProg[(int)Eprog.A];
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.F)
                {
                    //wywołaj metode F klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.B)
                {
                    rejestry.lr++;
                    Console.Write("SVC B");
                    Mem.XB();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.C)
                {
                    rejestry.lr++;
                    Console.Write("SVC C");
                    Proc.XC();
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
                    //wywołaj metode Y klasy Proc (SVC, Y, REJESTR|PROG, Eprog)
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.Z)
                {
                    //wywołaj metode Z klasy Proc
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.Q)
                {
                    //wywołaj metode Q klasy Proc
                }
            }//SVC do dokończenia (brak wywołań) dodanie do stosu licznika rozkazów
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.MOV)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//MOV R0 MEM (przepisuje 2 komórki pamięci)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.r1];
                        temp=temp << 8;
                        temp += Mem.MEMORY[(int)rejestry.r1+1];
                        rejestry.r0 = temp;
                        
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)//MOV R0 WART 0,0,
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r0 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r0 =rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r0 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//MOV R1 MEM (przepisuje 2 komórki)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r1 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)//MOV R1 WART 0,0,
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r1 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r1 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r2 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r2 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r2 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r3 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r3 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r3 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.lr = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        
                        temp += Mem.MEMORY[(int)rejestry.lr+1];
                        rejestry.lr = temp;
                        
                        
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.lr = (int)rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                {
                     rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r4 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r4 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r4 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r5 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r5 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r5 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r6 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r6 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r6 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r7 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r7 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r8;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r7 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r8 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r8 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {
                        rejestry.lr++;
                        rejestry.r8 = rejestry.r9;
                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                    {
                        rejestry.lr++;
                        int temp2 = (int)rejestry.r1;
                        int temp = Mem.MEMORY[temp2];
                        temp = temp << 8;
                        temp += Mem.MEMORY[temp2 + 1];
                        rejestry.r9 = temp;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        int temp = Mem.MEMORY[(int)rejestry.lr];
                        temp = temp << 8;
                        rejestry.lr++;
                        temp += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.r9 = temp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r2;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.lr;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r4;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r5;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r6;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r7;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {
                        rejestry.lr++;
                        rejestry.r9 = rejestry.r8;
                    }
                    
                }

                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes(rejestry.lr);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;

                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        Mem.MEMORY[(int)rejestry.r1] = Mem.MEMORY[(int)rejestry.lr];
                        rejestry.lr++;

                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r0);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r1);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r2);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r3);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r4);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r5);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r6);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r7);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r8);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                    {

                        byte tmp = 0;
                        byte tmp2 = 0;
                        byte[] bytes = BitConverter.GetBytes((int)rejestry.r9);
                        if (BitConverter.IsLittleEndian)
                        {
                            tmp = bytes[0];
                            tmp2 = bytes[1];
                        }
                        else
                        {
                            tmp = bytes[3];
                            tmp2 = bytes[2];
                        }
                        Mem.MEMORY[(int)rejestry.r1] = tmp2;
                        Mem.MEMORY[(int)rejestry.r1 + 1] = tmp;
                        rejestry.lr++;


                    }
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.SEM)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SEM.MEMORY)
                    {
                        rejestry.lr++;
                        rejestry.r2 = Mem.MEMORY_SEM;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SEM.FSBSEM)
                    {
                        rejestry.lr++;
                        rejestry.r2 = Mem.FSBSEM;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SEM.COMMON)
                    {
                        rejestry.lr++;
                        rejestry.r2 = zawiadowca.RUNNING.MESSAGE_SEMAPHORE_COMMON;

                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SEM.RECEIVER)
                    {
                        rejestry.lr++;
                        rejestry.r2 = zawiadowca.RUNNING.MESSAGE_SEMAPHORE_RECEIVER;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SEM.R2_COMMON)
                    {
                        rejestry.lr++;
                        rejestry.r2 = ((PCB)rejestry.r2).MESSAGE_SEMAPHORE_COMMON;

                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SEM.R2_RECEIVER)
                    {
                        rejestry.lr++;
                        rejestry.r2 = ((PCB)rejestry.r2).MESSAGE_SEMAPHORE_RECEIVER;
                    }
                }
            }//MOV kopiuje wartość z wybranego miejsca do innego                                (MOV, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|SEM, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART|***, *** wyjaśnienie wyżej)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.ADD)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)//ADD WART 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += Mem.MEMORY[(int)rejestry.lr];
                    rejestry.r0 = temp;
                    rejestry.lr++;

                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//ADD MEM (wskazywana przez r1) 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += Mem.MEMORY[(int)rejestry.r1];
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)//ADD R0 dodaje akumulator do akumulatora (2*r0)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r0;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r1;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r2;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r3;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)// (dodaje licznik rozkazów po przesunięciu go na następna komórkę)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.lr;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r4;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r5;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r6;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r7;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r8;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)//
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r9;
                    rejestry.r0 = temp;
                }


            }//ADD dodawanie do akumulatora,                                                    (ADD, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART, <wartosc jeżeli wcześniej WART>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.SUB)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)//SUB WART 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= Mem.MEMORY[(int)rejestry.lr];
                    rejestry.r0 = temp;
                    rejestry.lr++;

                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//SUB MEM (wskazywana przez r1) 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= Mem.MEMORY[(int)rejestry.r1];
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)//SUB R0 odejmuje akumulator od akumulatora (r0=0)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r0;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//SUB R1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r1;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//SUB R2
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r2;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//SUB R3
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r3;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)//SUB LR (dodaje licznik rozkazów po przesunięciu go na następna komórkę)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.lr;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)//SUB
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r4;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)//SUB
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r5;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)//SUB
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r6;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)//SUB
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r7;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)//SUB
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r8;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)//SUB
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp -= (int)rejestry.r9;
                    rejestry.r0 = temp;
                }
            }//SUB odejmowanie od akumulatora,                                                  (SUB, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART, <wartosc jeżeli wcześniej WART>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.MUL)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)//MUL WART 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= Mem.MEMORY[(int)rejestry.lr];
                    rejestry.r0 = temp;
                    rejestry.lr++;

                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//MUL MEM (wskazywana przez r1) 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= Mem.MEMORY[(int)rejestry.r1];
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)//MUL R0 mnoży akumulator przez akumulator (r0^2)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r0;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//MUL R1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r1;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//MUL R2
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r2;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//MUL R3
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r3;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)//MUL LR (mnoży licznik rozkazów po przesunięciu go na następna komórkę)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.lr;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)//MUL
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r4;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)//MUL
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r5;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)//MUL
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r6;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)//MUL
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r7;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)//MUL
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r8;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)//MUL
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp *= (int)rejestry.r9;
                    rejestry.r0 = temp;
                }
            }//MUL mnożenie akumulatora,                                                        (MUL, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART, <wartosc jeżeli wcześniej WART>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.DIV)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)//DIV WART 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= Mem.MEMORY[(int)rejestry.lr];
                    rejestry.r0 = temp;
                    rejestry.lr++;

                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//DIV MEM (wskazywana przez r1) 
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= Mem.MEMORY[(int)rejestry.r1];
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)//DIV R0 dzieli akumulator do akumulatora (r0=1)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r0;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//DIV R1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r1;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//DIV R2
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r2;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//DIV R3
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r3;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)//DIV LR (dzieli przez licznik rozkazów po przesunięciu go na następna komórkę)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.lr;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)//DIV
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r4;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)//DIV
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r5;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)//DIV
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r6;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)//DIV
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r7;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)//DIV
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r8;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)//DIV
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp /= (int)rejestry.r9;
                    rejestry.r0 = temp;
                }
            }//DIV dzielenie akumulatora, bez reszty,                                           (DIV, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART, <wartosc jeżeli wcześniej WART>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.INC)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)//zwiększa akumulator o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp++;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//zwiększa rejestr 1 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r1;
                    temp++;
                    rejestry.r1 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//zwiększa rejestr 2 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r2;
                    temp++;
                    rejestry.r2 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//zwiększa rejestr 3 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r3;
                    temp++;
                    rejestry.r3 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)//zwiększa rejestr lr o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.lr;
                    temp++;
                    rejestry.lr = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//zwiększa komórkę pamięci wksazywaną przez r1 o 1
                {
                    rejestry.lr++;
                    byte temp = Mem.MEMORY[(int)rejestry.r1];
                    temp++;
                    Mem.MEMORY[(int)rejestry.r1] = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)//zwiększa rejestr 4 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r4;
                    temp++;
                    rejestry.r4 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)//zwiększa rejestr 5 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r5;
                    temp++;
                    rejestry.r5 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)//zwiększa rejestr 6 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r6;
                    temp++;
                    rejestry.r6 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)//zwiększa rejestr 7 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r7;
                    temp++;
                    rejestry.r7 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)//zwiększa rejestr 8 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r8;
                    temp++;
                    rejestry.r8 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)//zwiększa rejestr 9 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r9;
                    temp++;
                    rejestry.r9 = temp;
                }
            }//INC inkrementacja,                                                               (INC, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.DEC)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)//zmniejsza akumulator o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp--;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//zmniejsza rejestr 1 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r1;
                    temp--;
                    rejestry.r1 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//zmniejsza rejestr 2 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r2;
                    temp--;
                    rejestry.r2 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//zmniejsza rejestr 3 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r3;
                    temp--;
                    rejestry.r3 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)//zmniejsza rejestr lr o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.lr;
                    temp--;
                    rejestry.lr = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)//zmniejsza komórkę pamięci wksazywaną przez r1 o 1
                {
                    rejestry.lr++;
                    byte temp = Mem.MEMORY[(int)rejestry.r1];
                    temp--;
                    Mem.MEMORY[(int)rejestry.r1] = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)//zmniejsza rejestr 4 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r4;
                    temp--;
                    rejestry.r4 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)//zmniejsza rejestr 5 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r5;
                    temp--;
                    rejestry.r5 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)//zmniejsza rejestr 6 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r6;
                    temp--;
                    rejestry.r6 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)//zmniejsza rejestr 7 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r7;
                    temp--;
                    rejestry.r7 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)//zmniejsza rejestr 8 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r8;
                    temp--;
                    rejestry.r8 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)//zmniejsza rejestr 9 o 1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r9;
                    temp--;
                    rejestry.r9 = temp;
                }
            }//DEC dekrementacja,                                                               (DEC, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART) 
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JUMPF)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                {
                    rejestry.lr++;
                    rejestry.lr += Mem.MEMORY[(int)rejestry.lr];
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.r1] == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r0 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r1 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r2 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r3 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r4 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r5 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r6 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r7 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r8 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r9 == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                {
                    rejestry.lr++;
                    if ((int)rejestry.lr == 0)
                    {

                        rejestry.lr += Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                //skacze w przód (gdy pierwsze jest wart to skok bezwarunkowy inaczej warunkowy z wskazaniem rejestru)
            }//JUMPF skok warunkowy lub nie w zależności od następnego parametru, do przodu     (JUMPF, R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART, <odległość>
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JUMPR)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                {
                    rejestry.lr++;
                    rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.r1] == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r0 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r1 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r2 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r3 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R4)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r4 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R5)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r5 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R6)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r6 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R7)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r7 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R8)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r8 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R9)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r9 == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                {
                    rejestry.lr++;
                    if ((int)rejestry.lr == 0)
                    {

                        rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];

                    }
                    rejestry.lr++;
                }
                //skacze w tył
            }//JUMPR skok warunkowy lub nie w zależności od następnego parametru, do tyłu       (JUMPR,  R0|R1|R2|R3|R4|R5|R6|R7|R8|R9|LR|MEM|WART, <odległość>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JUMPV)
            {
                if ((int)rejestry.r0 != 0)
                {
                    rejestry.lr++;
                    rejestry.lr -= Mem.MEMORY[(int)rejestry.lr];
                    rejestry.lr++;
                }
            }//JUMPV skok gdy r0!=0 do tyłu o daną wartość
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.METHOD)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.CZYSC_PODR)
                {
                    rejestry.lr++;
                    int adr = 0;
                    int dl = 0;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        adr = (int)rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        adr = (int)rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        adr = (int)rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        adr = (int)rejestry.r2;
                    }

                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        dl = Mem.MEMORY[(int)rejestry.lr] << 8;
                        rejestry.lr++;
                        dl += Mem.MEMORY[(int)rejestry.lr];
                        rejestry.lr++;
                    }
                    IBSUP.czyscPodr(adr, dl);
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.PRZYG_XR)
                {
                    rejestry.lr++;
                    int adr = 0;
                    int dl = 0;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)
                    {
                        adr = (int)rejestry.r3;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R0)
                    {
                        adr = (int)rejestry.r0;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)
                    {
                        adr = (int)rejestry.r1;
                    }
                    else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)
                    {
                        adr = (int)rejestry.r2;
                    }

                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                    {
                        rejestry.lr++;
                        dl = Mem.MEMORY[(int)rejestry.lr];
                        rejestry.lr++;

                    }
                    IBSUP.przygXR(adr, dl);
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.INTER_KOM)
                {
                    rejestry.lr++;
                    IBSUP.interKom();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.SPRAWDZENIE)
                {
                    rejestry.lr++;
                    Ext.SPRAWDZENIE();
                }
               /* else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.CZYTNIK)
                {
                    rejestry.lr++;
                    Ext.CZYTNIK(Ext.com, Ext.adres, Ext.il_danych);
                }*/
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.SCAN)
                {
                    rejestry.lr++;
                    IBSUP.SCAN();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.PRZESZUKAJ_LISTE)
                {
                    rejestry.lr++;
                    Mem.PRZESZUKAJ_LISTE();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.PODRECZNA)
                {
                    rejestry.lr++;
                    Mem.PODRECZNA();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.POCZATEK_MEM)
                {
                    rejestry.lr++;
                    Mem.POCZATEK_MEM();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.KONIEC_MEM)
                {
                    rejestry.lr++;
                    Mem.KONIEC_MEM();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.READ_MSG)
                {
                    rejestry.lr++;
                    IBSUP.READ_MSG();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.INTER_LOAD)
                {
                    rejestry.lr++;
                    IBSUP.INTER_LOAD();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.PRINT_MSG)
                {
                    rejestry.lr++;
                    IBSUP.PRINT_MSG();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.EXPUNGE1)
                {
                    rejestry.lr++;
                    IBSUP.EXPUNGE1();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.EXPUNGE2)
                {
                    rejestry.lr++;
                    IBSUP.EXPUNGE2();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.EXPUNGE3)
                {
                    rejestry.lr++;
                    IBSUP.EXPUNGE3();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.EXPUNGE4)
                {
                    rejestry.lr++;
                    IBSUP.EXPUNGE4();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.GRUPA)
                {
                    rejestry.lr++;
                    Ext.GRUPA();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.ZERUJ_PAM)
                {
                    rejestry.lr++;
                    IBSUP.ZERUJ_PAM();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.XF)
                {
                    rejestry.lr++;
                    Mem.XFW();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.XA)
                {
                    rejestry.lr++;
                    Mem.XAW();
                }

                //dokończyć
            }//METHOD możliwe rozszerzenia                                                      (METHOD, <nazwa metody>, <opcjonalnie parametry>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.FLAG)
            {
                rejestry.lr++;
                int tmp = Mem.MEMORY[rejestry.lr];
                rejestry.lr++;
                LFlag[tmp] = (byte)rejestry.lr;

            }//FLAG dodanie flagi (zapamiętanie licznika rozkazów)                              (FLAG , <nr flagi>)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JZ)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r0 == 0)
                        rejestry.lr = LFlag[(int)Mem.MEMORY[rejestry.lr]];

                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.PROG)
                {
                    rejestry.lr++;
                    if ((int)rejestry.r0 == 0)
                        rejestry.lr = IPLRTN.adrProg[(int)Mem.MEMORY[rejestry.lr]];

                    rejestry.lr++;
                }
            }//JZ skok przy r0==0 do flagi działa                                               (JZ, WART|PROG, numer flagi lub nazwa programu)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JMP)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                {
                    rejestry.lr++;
                    rejestry.lr = LFlag[(int)Mem.MEMORY[rejestry.lr]];
                    rejestry.lr++;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.PROG)
                {
                    rejestry.lr++;
                    rejestry.lr = IPLRTN.adrProg[(int)Mem.MEMORY[rejestry.lr]];
                    rejestry.lr++;
                }
            }//JMP skok bezwarunkowy do flagi                                                   (JMP, WART|PROG, numer flagi lub nazwa programu)
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.POWROT)
            {
                int tmp = stos.Pop();
                rejestry.lr = tmp;
                //odczytanie ze stosu licznika rozkazów i go ustawienie
            }//POWROT wczytuje licznik rozkazów ze stosu
            

        }
    }

   
    
}
