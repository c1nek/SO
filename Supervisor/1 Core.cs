using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpreter;

namespace Processor
{
    public static class zawiadowca
    {
        public static List<PCB> listaWszystichPCB = new List<PCB>();
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
        public PCB NEXT_SEMAPHORE_WAITER;
        public object[] cpu_stan = new object[5];

        public void cpu_stan_zapisz()
        {
            cpu_stan[0] = rejestry.get_r0();
            cpu_stan[1] = rejestry.get_r1();
            cpu_stan[2] = rejestry.get_r2();
            cpu_stan[3] = rejestry.get_r3();
            cpu_stan[4] = rejestry.get_lr();
        }

        public void cpu_stan_laduj()
        {
            rejestry.set_r0(cpu_stan[0]);
            rejestry.set_r1(cpu_stan[1]);
            rejestry.set_r2(cpu_stan[2]);
            rejestry.set_r3(cpu_stan[3]);
            rejestry.set_lr(cpu_stan[4]);
        }

        public PCB(string name, int time)
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
        }
    }

    public static  class rejestry
    {
        public static object r0; //akumulator
        public static object r1;
        public static object r2;
        public static object r3;
        public static object lr;


        //to należy wywalićVVVVV
        /*getery*/
        public static object get_r0()
        {
            return r0;
        }

        public static object get_r1()
        {
            return r1;
        }

        public static object get_r2()
        {
            return r2;
        }

        public static object get_r3()
        {
            return r3;
        }

        public static object get_lr()
        {
            return lr;
        }


        /*settery*/
        public static void set_r0(object value)
        {
            r0 = value;
        }

        public static void set_r1(object value)
        {
            r1 = value;
        }

        public static void set_r2(object value)
        {
            r2 = value;
        }

        public static void set_r3(object value)
        {
            r3 = value;
        }

        public static void set_lr(object value)
        {
            lr = value;
        }


    }


    //do poprawki!!!!!VVVVVVVVV
    public class SEMAPHORE
    {
        private int VALUE;
        private List<PCB> semaphoreList = new List<PCB>();
        public PCB FIRST_WAITER=null;
        public SEMAPHORE()
        {
            VALUE = 0;
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

        public static void v()
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


