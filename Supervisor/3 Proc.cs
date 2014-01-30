
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;
using Memory;

namespace Process
{
    public static class Proc
    {

        public enum rozkaz : byte { SVC, MOV, ADD, SUB, MUL, DIV, INC, DEC, JUMPF, JUMPR, JZ, JMP, METHOD, FLAG, POWROT, KONIEC, JUMPV };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, LR, MEM, WART, SEM, PROG };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY, POCZATEK_MEM, KONIEC_MEM, GRUPA, ZERUJ_PAM, XA, XF, XD, XR, XS };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };

 
          public static void XC()//utworzenie procesu
        {
            object r2 = rejestry.r2;
            int i = 0;
            UTF8Encoding kodowanie = new UTF8Encoding();
            byte[] c = new byte[8];
            for (; i < 8 && Mem.MEMORY[((int)rejestry.r2) + i] != 0; i++)
            {
                c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
            }
            string nazwa = kodowanie.GetString(c, 0, i);

            PCB nowy = new PCB(nazwa);
            nowy.STOPPED = true;
            nowy.BLOCKED = false;
            rejestry.r2 = nowy;
            XI();
            
              rejestry.r2 = r2;
        }

         private static object zap = null;
        public static void XDM()
        {
            if(zap==null)
            {
            int i = 0;
            UTF8Encoding kodowanie = new UTF8Encoding();
            byte[] c = new byte[8];
            for (; i < 8 && Mem.MEMORY[((int)rejestry.r2) + i] != 0; i++)
            {
                c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
            }
            string nazwa = kodowanie.GetString(c, 0, i);
            Console.WriteLine("Usuwanie procesu {0}", nazwa);
            PCB usun = XN(nazwa);
            XJ(usun);
             zap = rejestry.r2;
            Mem.MEMORY[(int)rejestry.r3 + 200] = 1;
            Mem.MEMORY[(int)rejestry.r3 + 201] = 0;
            byte[] tmpB = BitConverter.GetBytes(usun.ADR_PODR);
            if (BitConverter.IsLittleEndian == true)
            {
                Mem.MEMORY[(int)rejestry.r3 + 202] = tmpB[1];
                Mem.MEMORY[(int)rejestry.r3 + 203] = tmpB[0];
            }
            else
            {
                Mem.MEMORY[(int)rejestry.r3 + 202] = tmpB[2];
                Mem.MEMORY[(int)rejestry.r3 + 203] = tmpB[3];
            }
            }
            else
            {
                rejestry.r2=zap;
                zap=null;
                
            }

        }            


        


        public static byte[] XD = new byte[]//tego raczej nie robimy
        {
            (byte)rozkaz.METHOD, (byte)wartosc_METHOD.XD,
            (byte)rozkaz.SVC,       (byte)wartosc_SVC.F,
            (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.XD,
            (byte)rozkaz.POWROT
        };
        public static int zaladujXD(int m)
        {
            int i;
            for (i = 0; i < XD.Length; i++)
            {
                Mem.MEMORY[i + m] = XD[i];
            }
            return m + i + 1;
        }

        public static PCB XN(string nazwa)
        {
            PCB tmp = zawiadowca.RUNNING;
            int j = 0;
            while (tmp != zawiadowca.RUNNING || j == 0)
            {
                if (tmp.NAME == nazwa)
                {
                    Console.WriteLine("XN: znaleziono PCB o nazwie {0}", nazwa);
                    return tmp;
                }
                tmp = tmp.NEXT_PCB_GROUP;
                j++;
            }
            Console.WriteLine("XN: nie znaleziono PCB o nazwie {0}", nazwa);
            return null;
        }
        

        public static byte[] XR = new byte[]
        {
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XR,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.RECEIVER,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.P,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.COMMON,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.P,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XR,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.COMMON,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.V,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.RECEIVER,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.V,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XR,
            (byte)rozkaz.POWROT
        };
        private static bool przywroc=false;
        private static object zap2 = null;
        public static void XRM()
        {
            if (zap2 == null)
            {
                Console.WriteLine("Odbieranie komunikatu");
                zap2 = rejestry.r2;
            }
            else if (przywroc)
            {
                rejestry.r2 = zap2;
            }
            else
            {
                string nadawca=zawiadowca.RUNNING.FIRST_MESSAGE.SENDER.NAME;
                int dl = zawiadowca.RUNNING.FIRST_MESSAGE.SIZE;
                byte [] tekst = zawiadowca.RUNNING.FIRST_MESSAGE.TEXT;
                int tmp = (int)zap2;
                UTF8Encoding kodowanie = new UTF8Encoding();
                kodowanie.GetBytes(nadawca,0,nadawca.Length,Mem.MEMORY,tmp);
                tmp += 8;
                if (BitConverter.IsLittleEndian)
                    Mem.MEMORY[tmp] = BitConverter.GetBytes(dl)[0];
                else
                    Mem.MEMORY[tmp] = BitConverter.GetBytes(dl)[3];
                Array.Copy(tekst, 0, Mem.MEMORY, tmp + 1, dl);
                MESSAGE tm = zawiadowca.RUNNING.FIRST_MESSAGE;
                zawiadowca.RUNNING.FIRST_MESSAGE = zawiadowca.RUNNING.FIRST_MESSAGE.NEXT;
                tm.NEXT = null;

                przywroc = true;
            }
        }

        public static int zaladujXR(int m)
        {
            int i;
            for (i = 0; i < XR.Length; i++)
            {
                Mem.MEMORY[i + m] = XR[i];
            }
            return m + i + 1;
        }

        public static byte[] XS = new byte[]
        {
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XS,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.R2_COMMON,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.P,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XS,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.R2_RECEIVER,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.P,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XS,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.R2_RECEIVER,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.V,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XS,
            (byte)rozkaz.MOV,   (byte)wartosc_TYP.SEM,  (byte)wartosc_SEM.R2_COMMON,
            (byte)rozkaz.SVC,   (byte)wartosc_SVC.V,
            (byte)rozkaz.METHOD,(byte)wartosc_METHOD.XS,
            (byte)rozkaz.POWROT
        };
        public static int zaladujXS(int m)
        {
            int i;
            for (i = 0; i < XS.Length; i++)
            {
                Mem.MEMORY[i + m] = XS[i];
            }
            return m + i + 1;
        }

        private static bool przywroc1 = false;
        private static object zap3 = null;
        private static int licz = 0;
        private static PCB odb;
        public static void XSM()
        {
            if (zap3 == null)
            {
                Console.WriteLine("Wysyłanie komunikatu");
                zap3 = rejestry.r2;

                int i = 0;
                UTF8Encoding kodowanie = new UTF8Encoding();
                byte[] c = new byte[8];
                for (; i < 8 && i != 0; i++)
                {
                    c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
                }
                string odbiorca = kodowanie.GetString(c, 0, i);
                odb=XN(odbiorca);
                rejestry.r2 = odb.MESSAGE_SEMAPHORE_COMMON;
            }
            else if (licz == 0)
            {
                rejestry.r2 = odb.MESSAGE_SEMAPHORE_RECEIVER;
                licz++;
            }
            else if (licz == 2)
            {
                rejestry.r2 = odb.MESSAGE_SEMAPHORE_COMMON;
                przywroc1 = true;
            }
            else if (przywroc1)
            {
                
                rejestry.r2 = zap3;
            }
            else
            {
                MESSAGE m = new MESSAGE();
                m.SIZE = Mem.MEMORY[(int)zap3 + 8];
                m.SENDER = zawiadowca.RUNNING;
                Array.Copy(Mem.MEMORY, (int)zap3 + 9, m.TEXT, 0, m.SIZE);
                byte[] tekst = zawiadowca.RUNNING.FIRST_MESSAGE.TEXT;
               
                MESSAGE tmp=odb.FIRST_MESSAGE;
                while (tmp.NEXT != null)
                {
                    tmp = tmp.NEXT;
                }
                tmp.NEXT = m;

                rejestry.r2 = odb.MESSAGE_SEMAPHORE_RECEIVER;
                licz = 2;
            }
        }

        public static void XY(int adr)
        {
            
            int i = 0;
            UTF8Encoding kodowanie = new UTF8Encoding();
            byte[] c = new byte[8];
            for (; i < 8 && Mem.MEMORY[((int)rejestry.r2) + i] != 0; i++)
            {
                c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
            }
            string nazwa = kodowanie.GetString(c, 0, i);
            Console.WriteLine("Usuwanie procesu {0}", nazwa);
            PCB wlacz = XN(nazwa);
            wlacz.cpu_stan_zapisz();
            wlacz.cpu_stan[4] = adr;
            wlacz.STOPPED = false;

        }

        public static void XZ()
        {
            int i = 0;
            UTF8Encoding kodowanie = new UTF8Encoding();
            byte[] c = new byte[8];
            for (; i < 8 && i != 0; i++)
            {
                c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
            }
            string nazwa = kodowanie.GetString(c, 0, i);
            int j = 0;
            PCB pomoc = zawiadowca.RUNNING;
            PCB pierwszy = zawiadowca.RUNNING;

            if (pierwszy != pomoc||j==0)
            {
            start3:
                if (pomoc.NAME == nazwa)
                {
                    pomoc.STOPPED = true;
                }
                else
                {
                    pomoc = pomoc.NEXT_PCB_GROUP;
                    goto start3;
                }
            j++;
            }
            else
            {
                Console.WriteLine("brak procesu o podanej nazwie do zatrzymania.");
            }
        }
        
        public static byte[] XQUE = new byte[]
        {

        };

        public static int zaladujXQUE(int m)
        {
            int i;
            for (i = 0; i < XQUE.Length; i++)
            {
                Mem.MEMORY[i + m] = XQUE[i];
            }
            return m + i + 1;
        }



        public static void XI()
        {
            //dolaczanie do grupy
            PCB wykonywany_grupa = zawiadowca.RUNNING;
            PCB ostatni_grupa = wykonywany_grupa.LAST_PCB_GROUP;
            PCB pomoc1_grupa = ostatni_grupa.NEXT_PCB_GROUP;
            PCB pomoc2_grupa = (PCB)rejestry.r2;
            ostatni_grupa.NEXT_PCB_GROUP = (PCB)rejestry.r2;
            pomoc2_grupa.NEXT_PCB_GROUP = pomoc1_grupa;
            pomoc2_grupa.LAST_PCB_GROUP = ostatni_grupa;

            //dolaczanie do calego lancucha
            PCB wykonywany_lancuch = zawiadowca.RUNNING;
            PCB ostatni_lancuch = wykonywany_lancuch.LAST_PCB_ALL;
            PCB pomoc1_lancuch = ostatni_lancuch.NEXT_PCB_ALL;
            PCB pomoc2_lancuch = (PCB)rejestry.r2;
            ostatni_lancuch.NEXT_PCB_ALL = (PCB)rejestry.r2;
            pomoc2_lancuch.NEXT_PCB_ALL = pomoc1_lancuch;
            pomoc2_lancuch.LAST_PCB_ALL = ostatni_lancuch;
        }

        public static void XJ(PCB x) //usuniecie bloku
        {
            //usuniecie z grupy
            PCB pomoc1_grupa = zawiadowca.RUNNING;
            PCB pomoc2_grupa = pomoc1_grupa.NEXT_PCB_GROUP;
            PCB pomoc3_grupa = pomoc2_grupa.NEXT_PCB_GROUP;
            PCB rejestr = x;

        start:
            if (pomoc2_grupa == rejestr)
            {
                pomoc1_grupa.NEXT_PCB_GROUP = pomoc3_grupa;
                Console.WriteLine("Usunieto z grupy blok PCB o nazwie " + rejestr.NAME);
            }
            else
            {
                pomoc1_grupa = pomoc1_grupa.NEXT_PCB_GROUP;
                pomoc2_grupa = pomoc2_grupa.NEXT_PCB_GROUP;
                pomoc3_grupa = pomoc3_grupa.NEXT_PCB_GROUP;
                goto start;
            }

            //usuniecie z lancucha
            PCB pomoc1_lancuch = zawiadowca.RUNNING;
            PCB pomoc2_lancuch = pomoc1_grupa.NEXT_PCB_ALL;
            PCB pomoc3_lancuch = pomoc2_grupa.NEXT_PCB_ALL;

        start2:
            if (pomoc2_grupa == rejestr)
            {
                pomoc1_grupa.NEXT_PCB_ALL = pomoc3_lancuch;
                Console.WriteLine("Usunieto z lancucha blok PCB o nazwie " + rejestr.NAME);
            }
            else
            {
                pomoc1_lancuch = pomoc1_grupa.NEXT_PCB_ALL;
                pomoc2_lancuch = pomoc2_grupa.NEXT_PCB_ALL;
                pomoc3_lancuch = pomoc3_grupa.NEXT_PCB_ALL;
                goto start2;
            }


        }


        
    }

    public class MESSAGE
    {
       public PCB SENDER;
       public MESSAGE NEXT;
       public int SIZE;
       public byte[] TEXT;
       public MESSAGE()
        {
            SENDER = null;
            NEXT = null;
            SIZE = 0;
            TEXT = new byte[255];
        }
    }
    
    
        
        
        

    
}


