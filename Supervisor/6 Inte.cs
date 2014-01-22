using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using Processor;
using Supervisor;
//do dokończenia
namespace Interpreter
{
    public static class Inter
    {
        public enum rozkaz : byte { SVC, ADD, MOV, DIV, SUB, INC, DEC, JUMPF, JUMPR, METHOD, CREATE };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_CREATE : byte { KOM, PCB };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, LR, MEM, WART, SEM };
        public enum wartosc_SEM : byte { MEMORY, USER, WAIT, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM };
        

        public static List<int> progAdr = new List<int>();
        


        public static void Run()
        {
            if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.SVC)
            {
                rejestry.lr++;

                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.P)
                {
                    //wywołaj metode P klasy semafor
                    rejestry.lr++;
                    SEMAPHORE.P();
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.V)
                {
                    rejestry.lr++;
                    SEMAPHORE.V();
                    //wywołaj metode V klasy semafor
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.G)
                {
                    rejestry.lr++;
                    return;
                    //wywołaj metode Run zawiadowcy
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.A)
                {
                    
                //to musi sie odbyć przez przesunięcie lr
                    //wywołaj metode A klasy Mem 
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.E)
                {
                    //to musi się odbys przez przesunięcie licznika rozkazów
                    //wywołaj metode E klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.F)
                {
                    //wywołaj metode F klasy Mem
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_SVC.B)
                {
                    rejestry.lr++;
                    Mem.XB();
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
            }//SVC do dokończenia (brak wywołań)
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
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R1)//ADD R1
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r1;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R2)//ADD R2
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r2;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.R3)//ADD R3
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.r3;
                    rejestry.r0 = temp;
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)//ADD LR (dodaje licznik rozkazów po przesunięciu go na następna komórkę)
                {
                    rejestry.lr++;
                    int temp = (int)rejestry.r0;
                    temp += (int)rejestry.lr;
                    rejestry.r0 = temp;
                }

                //dokończyć

            }//ADD gotowe
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
                        rejestry.r2 = temp;
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
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.MEM)
                {
                    rejestry.lr++;
                    if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.LR)
                    {
                        byte tmp=0;
                        byte tmp2=0;
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
                        Mem.MEMORY[(int)rejestry.r1+1] = tmp;
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
                }

                //MOV Gotowe (tylko R0,R1,R2,R3,LR,MEM,WART) bez sem
                
               
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
                
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.DEC)
            {
                rejestry.lr++;
                //dokończyć
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JUMPF)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                {
                    rejestry.lr++;
                    rejestry.lr += Mem.MEMORY[(int)rejestry.lr];
                    rejestry.lr++;
                }
                
                //skacze w przód
            }
            else if (Mem.MEMORY[(int)rejestry.lr] == (byte)rozkaz.JUMPR)
            {
                rejestry.lr++;
                if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_TYP.WART)
                {
                    rejestry.lr++;
                    rejestry.lr = Mem.MEMORY[(int)rejestry.lr];
                    rejestry.lr++;
                }
                //skacze w tył
            }
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
                    IBSUB.czyscPodr(adr, dl);
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
                    IBSUB.przygXR(adr, dl);
                }
                else if (Mem.MEMORY[(int)rejestry.lr] == (byte)wartosc_METHOD.INTER_KOM)
                {
                    rejestry.lr++;
                    IBSUB.interKom();
                }
                
                //dokończyć
            }

        }
    }


    public class PAdr
    {
        public string name;
        int addr;
        PAdr(string n, int a)
        {
            name = n;
            addr = a;
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
