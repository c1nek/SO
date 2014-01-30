
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

          public static void XC()//utworzenie procesu
        {
            int i = 0;
            UTF8Encoding kodowanie = new UTF8Encoding();
            byte[] c = new byte[8];
            for (; i < 8 && i != 0; i++)
            {
                c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
            }
            string nazwa = kodowanie.GetString(c, 0, i);

            PCB nowy = new PCB(nazwa);
            nowy.STOPPED = true;
            nowy.BLOCKED = false;
            rejestry.r2 = nowy;
            XI();
        }

        public static void XD()
        {
            int i = 0;
            UTF8Encoding kodowanie = new UTF8Encoding();
            byte[] c = new byte[8];
            for (; i < 8 && i != 0; i++)
            {
                c[i] = Mem.MEMORY[((int)rejestry.r2) + i];
            }
            string nazwa = kodowanie.GetString(c, 0, i);
            PCB usun = XN(nazwa);
            
        }            

        


        public static byte[] XH = new byte[]//tego raczej nie robimy
        {

        };
        public static int zaladujXH(int m)
        {
            int i;
            for (i = 0; i < XH.Length; i++)
            {
                Mem.MEMORY[i + m] = XH[i];
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

        };
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

        public static byte[] XY = new byte[]
        {

        };

        public static int zaladujXY(int m)
        {
            int i;
            for (i = 0; i < XY.Length; i++)
            {
                Mem.MEMORY[i + m] = XY[i];
            }
            return m + i + 1;
        }

        public static byte[] XZ = new byte[]
        {

        };
        public static int zaladujXZ(int m)
        {
            int i;
            for (i = 0; i < XZ.Length; i++)
            {
                Mem.MEMORY[i + m] = XZ[i];
            }
            return m + i + 1;
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

        public static void XJ() //usuniecie bloku
        {
            //usuniecie z grupy
            PCB pomoc1_grupa = zawiadowca.RUNNING;
            PCB pomoc2_grupa = pomoc1_grupa.NEXT_PCB_GROUP;
            PCB pomoc3_grupa = pomoc2_grupa.NEXT_PCB_GROUP;
            PCB rejestr = (PCB)rejestry.r2;

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
            TEXT = null;
        }
    }
    
    
        
        
        

    
}


