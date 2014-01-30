using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;
using Process;
using External;
using Supervisor;

namespace Processor
{
    public static class zawiadowca
    {
        

        public static PCB RUNNING = null;
        public static PCB NEXTTRY = null;
        public static bool NEXTTRY_MODIFIED = false;
        public static bool wymusZmiane = false;
        public static int licznik = 0;


        public static void Run()
        {
            while (true)
            {
                Format.LicznikWrite();//wypisanie aktualnej wartości licznka zawiadowcy
                Format.RejestyWrite();

                Format.CWrite(ConsoleColor.Green, "Zawiadowca");
                Console.ReadLine();

                
                
                
                
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
                licznik++;
            }
        }

    }


    public class PCB
    {
        /*PCB*/
        public string NAME;
        public bool STOPPED;
        public bool BLOCKED;
        public bool PAM_PODR;
        public int ADR_PODR;
        public PCB NEXT_PCB_ALL;
        public PCB LAST_PCB_ALL;
        public PCB NEXT_PCB_GROUP;
        public PCB LAST_PCB_GROUP;
        public SEMAPHORE MESSAGE_SEMAPHORE_COMMON;
        public SEMAPHORE MESSAGE_SEMAPHORE_RECEIVER;
        public PCB NEXT_SEMAPHORE_WAITER;
        public MESSAGE FIRST_MESSAGE;
        public object[] cpu_stan = new object[12];

        public void cpu_stan_zapisz()
        {
            cpu_stan[0] = rejestry.r0;
            cpu_stan[1] = rejestry.r1;
            cpu_stan[2] = rejestry.r2;
            cpu_stan[3] = rejestry.r3;
            cpu_stan[4] = rejestry.lr;
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
            rejestry.r4 = cpu_stan[6];
            rejestry.r5 = cpu_stan[7];
            rejestry.r6 = cpu_stan[8];
            rejestry.r7 = cpu_stan[9];
            rejestry.r8 = cpu_stan[10];
            rejestry.r9 = cpu_stan[11];
        }

        public PCB(string name)
        {
            
            NAME = name;
            BLOCKED = false;
            STOPPED = false;
            NEXT_PCB_ALL = null;
            LAST_PCB_ALL = null;
            NEXT_PCB_GROUP = null;
            LAST_PCB_GROUP = null;
            MESSAGE_SEMAPHORE_COMMON=new SEMAPHORE(1,"COMMON");
            MESSAGE_SEMAPHORE_RECEIVER=new SEMAPHORE(1,"RECEIVER");
            PAM_PODR = false;
            ADR_PODR = 0;
        }
    }

    public static  class rejestry
    {
        public static object r0 = 0; //akumulator
        public static object r1 = 0;
        public static object r2 = 0;
        public static object r3 = 0;
        public static object r4 = 0;
        public static object r5 = 0;
        public static object r6 = 0;
        public static object r7 = 0;
        public static object r8 = 0;
        public static object r9 = 0;
        public static int lr = 0;    //licznik rozkazów





    }




    public class SEMAPHORE
    {
        public int VALUE;
        public string NAME;
        public List<PCB> semaphoreList = new List<PCB>();
        public PCB FIRST_WAITER = null;
        public SEMAPHORE(string name)
        {
            VALUE = 0;
            NAME = name;
        }
        public SEMAPHORE(int i, string name)
        {
            VALUE = i;
            NAME = name;
        }

        public static void P()//jeżeli semafor bedzie niedodatni to wtedy następuje blokowanie procesu który jest w RUNNING
        {
            Console.WriteLine("Wykonano operacje P na semaforze " + ((SEMAPHORE)rejestry.r2).NAME);
            SEMAPHORE S = (SEMAPHORE)rejestry.r2;
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
            Console.WriteLine("Wykonano operacje V na semaforze " + ((SEMAPHORE)rejestry.r2).NAME);
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
                S.FIRST_WAITER = S.FIRST_WAITER.NEXT_SEMAPHORE_WAITER;
                S.semaphoreList[0].NEXT_SEMAPHORE_WAITER = null;
                S.semaphoreList.RemoveAt(0);
                return;

            }
            return;
        }
    }


}


