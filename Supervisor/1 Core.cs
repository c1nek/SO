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
        private static void CWrite(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }

        private static void CWrite(ConsoleColor color, int text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }

        private static void LicznikWrite()
        {
            int kolumnaOrg = Console.CursorLeft;
            int wierszOrg = Console.CursorTop;
            Console.CursorLeft = 80;
            Console.CursorTop = 0;
            Console.Write("LZ: ");
            CWrite(ConsoleColor.Magenta,licznik);
            Console.CursorLeft = kolumnaOrg;
            Console.CursorTop = wierszOrg;
        }

        private static void RejestyWrite()
        {
            int kolumnaOrg = Console.CursorLeft;
            int wierszOrg = Console.CursorTop;
            Console.CursorLeft = 80;
            Console.CursorTop = 1;
            Console.Write("R0: ");
            if(rejestry.r0 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r0);
            else if(rejestry.r0 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r0).NAME);
            else if(rejestry.r0 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, ((SEMAPHORE)rejestry.r0).ToString());
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 2;
            Console.Write("R1: ");
            if (rejestry.r1 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r1);
            else if (rejestry.r1 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r1).NAME);
            else if (rejestry.r1 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, ((SEMAPHORE)rejestry.r1).ToString());
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 3;
            Console.Write("R2: ");
            if(rejestry.r2 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r2);
            else if(rejestry.r2 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r2).NAME);
            else if(rejestry.r2 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, ((SEMAPHORE)rejestry.r2).ToString());
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 4;
            Console.Write("R3: ");
            if(rejestry.r3 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r3);
            else if(rejestry.r3 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r3).NAME);
            else if(rejestry.r3 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, ((SEMAPHORE)rejestry.r3).ToString());
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 5;
            Console.Write("R4: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.r4);
            Console.CursorLeft = 80;
            Console.CursorTop = 6;
            Console.Write("R5: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.r5);
            Console.CursorLeft = 80;
            Console.CursorTop = 7;
            Console.Write("R6: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.r6);
            Console.CursorLeft = 80;
            Console.CursorTop = 8;
            Console.Write("R7: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.r7);
            Console.CursorLeft = 80;
            Console.CursorTop = 9;
            Console.Write("R8: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.r8);
            Console.CursorLeft = 80;
            Console.CursorTop = 10;
            Console.Write("R9: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.r9);
            Console.CursorLeft = 80;
            Console.CursorTop = 11;
            Console.Write("LR: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.lr);

            Console.CursorLeft = kolumnaOrg;
            Console.CursorTop = wierszOrg;
        }

        public static PCB RUNNING = null;
        public static PCB NEXTTRY = null;
        public static bool NEXTTRY_MODIFIED = false;
        public static bool wymusZmiane = false;
        private static int licznik = 0;


        public static void Run()
        {
            while (true)
            {
                LicznikWrite();//wypisanie aktualnej wartości licznka zawiadowcy
                RejestyWrite();

                CWrite(ConsoleColor.Green, "Zawiadowca");
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
        public static object r0 = 0; //akumulator
        public static object r1 = 0;
        public static object r2 = 0;
        public static object r3 = 0;
        public static Int16 r4 = 0;
        public static Int16 r5 = 0;
        public static Int16 r6 = 0;
        public static Int16 r7 = 0;
        public static Int16 r8 = 0;
        public static Int16 r9 = 0;
        public static int lr = 0;    //licznik rozkazów
        public static bool ab = false;  //adresowanie bezwzględne




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


