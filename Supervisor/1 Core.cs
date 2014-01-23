using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;
using Process;
using External;

namespace Processor
{
    public static class zawiadowca
    {
        public static PCB RUNNING = null;
        public static PCB NEXTTRY = null;
        public static bool NEXTTRY_MODIFIED = false;
        public static bool wymusZmiane = false;
        private static int licznik = 0;
        public static void Run()
        {
            while (true)
            {
                licznik++;
                if (licznik == 50 || wymusZmiane)
                {
                    wymusZmiane = false;
                    NEXTTRY_MODIFIED = false;
                    bool i = true;
                    RUNNING.cpu_stan_zapisz();
                    while (i)
                    {
                        if (!NEXTTRY.BLOCKED)
                        {
                            if (!NEXTTRY.STOPPED)
                            {
                                RUNNING = NEXTTRY;
                                NEXTTRY = RUNNING.NEXT_PCB_ALL;
                                RUNNING.cpu_stan_laduj();
                                i = false;
                                licznik=0;
                            }
                        }
                    }
                }
                Inter.Run();
            }
        }

    }


    public class PCB
    {
        /*PCB*/
        public string NAME;
        public bool STOPPED;
        public bool BLOCKED;
        public int instruction_done;
        public bool czy_sprawdzony;
        public string[] MEMORY_BLOCK;
        public PCB NEXT_PCB_ALL;
        public PCB LAST_PCB_ALL;
        public PCB NEXT_PCB_GROUP;
        public PCB LAST_PCB_GROUP;
        public UCB blokExt;
        public SEMAPHORE MESSAGE_SEMAPHORE_COMMON;
        public SEMAPHORE MESSAGE_SEMAPHORE_RECEIVER;
        public PCB NEXT_SEMAPHORE_WAITER;
        public MESSAGE FIRST_MESSAGE;
        public int adres_pocz;
        public object[] cpu_stan = new object[12];

        public void cpu_stan_zapisz()
        {
            cpu_stan[0] = rejestry.r0;
            cpu_stan[1] = rejestry.r1;
            cpu_stan[2] = rejestry.r2;
            cpu_stan[3] = rejestry.r3;
            cpu_stan[4] = rejestry.lr;
            cpu_stan[5] = rejestry.ab;
            cpu_stan[6] = rejestry.r4;
            cpu_stan[7] = rejestry.r5;
            cpu_stan[8] = rejestry.r6;
            cpu_stan[9] = rejestry.r7;
            cpu_stan[10] = rejestry.r8;
            cpu_stan[11] = rejestry.r9;


        }

        public void cpu_stan_laduj()
        {
            rejestry.r0 = cpu_stan[0];
            rejestry.r1 = cpu_stan[1];
            rejestry.r2 = cpu_stan[2];
            rejestry.r3 = cpu_stan[3];
            rejestry.lr = (int)cpu_stan[4];
            rejestry.ab = (bool)cpu_stan[5];
            rejestry.r4 = (Int16)cpu_stan[6];
            rejestry.r5 = (Int16)cpu_stan[7];
            rejestry.r6 = (Int16)cpu_stan[8];
            rejestry.r7 = (Int16)cpu_stan[9];
            rejestry.r8 = (Int16)cpu_stan[10];
            rejestry.r9 = (Int16)cpu_stan[11];
        }

        public PCB(string name, int adres)
        {
            
            NAME = name;
            BLOCKED = false;
            STOPPED = false;
            instruction_done = 0;
            czy_sprawdzony = true;
            NEXT_PCB_ALL = null;
            LAST_PCB_ALL = null;
            NEXT_PCB_GROUP = null;
            LAST_PCB_GROUP = null;
            MESSAGE_SEMAPHORE_COMMON=new SEMAPHORE(1);
            MESSAGE_SEMAPHORE_RECEIVER=new SEMAPHORE(1);
            adres_pocz = adres;
        }
    }

    public static  class rejestry
    {
        public static object r0; //akumulator
        public static object r1;
        public static object r2;
        public static object r3;
        public static Int16 r4;
        public static Int16 r5;
        public static Int16 r6;
        public static Int16 r7;
        public static Int16 r8;
        public static Int16 r9;
        public static int lr;    //licznik rozkazów
        public static bool ab;  //adresowanie bezwzględne




    }


    public class SEMAPHORE
    {
        private int VALUE;
        private List<PCB> semaphoreList = new List<PCB>();
        public PCB FIRST_WAITER=null;
        public SEMAPHORE()
        {
            VALUE = 0;
        }
        public SEMAPHORE(int i)
        {
            VALUE = i;
        }

        public static void P()//jeżeli semafor bedzie niedodatni to wtedy następuje blokowanie procesu który jest w RUNNING
        {
            SEMAPHORE S = (SEMAPHORE) rejestry.r2;
            S.VALUE--;
            if (S.VALUE < 0)
            {
                S.semaphoreList.Add(zawiadowca.RUNNING);
                if (S.VALUE == (-1))
                    S.FIRST_WAITER = zawiadowca.RUNNING;
                else
                {
                    PCB temp = S.semaphoreList.Last();
                    temp.NEXT_SEMAPHORE_WAITER = zawiadowca.RUNNING;
                }
                zawiadowca.RUNNING.BLOCKED = true;
                zawiadowca.wymusZmiane = true;
                return;
            }
            else
            {
                return;
            }

        }

        public static void V()
        {
            SEMAPHORE S = (SEMAPHORE)rejestry.r2;
            S.VALUE++;
            if (S.VALUE <= 0)
            {
                S.FIRST_WAITER.BLOCKED = false;
                if (zawiadowca.NEXTTRY_MODIFIED == false)
                {
                    zawiadowca.NEXTTRY = S.FIRST_WAITER;
                    zawiadowca.NEXTTRY_MODIFIED = true;
                }
                S.FIRST_WAITER=S.FIRST_WAITER.NEXT_SEMAPHORE_WAITER;
                S.semaphoreList[0].NEXT_SEMAPHORE_WAITER = null;
                S.semaphoreList.RemoveAt(0);
                return;

            }
            return;
        }




    }


}


