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
               // Console.ReadLine();

                
                if (licznik==50||wymusZmiane==true)
                {
                    
                    wymusZmiane = false;
                    NEXTTRY_MODIFIED = false;
                    bool i = true;
                    RUNNING.cpu_stan_zapisz();
                    Console.Clear();
                    while (i)              
                    
                                      
                    
                   {
                        if (!NEXTTRY.BLOCKED && !NEXTTRY.STOPPED)
                        {

                            
                            
                            RUNNING = NEXTTRY;
                            NEXTTRY = RUNNING.NEXT_PCB_ALL;
                            
                            RUNNING.cpu_stan_laduj();
                            i = false;
                            licznik = 0;
                            Console.WriteLine("Uruchomiony proces {0}", RUNNING.NAME);

                        }
                        else
                        {
                            NEXTTRY = NEXTTRY.NEXT_PCB_ALL;
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
        public Queue <MESSAGE> FIRST_MESSAGE;
        public Stack<int> stos = new Stack<int>();
        public object[] cpu_stan = new object[12];
        public int[] LFlag = new int[100];
        public bool przywroc_z;
        public  object zap2_Z;
        public object zap_Z;
        public bool przywroc1_z;
        public object zap3_z ;
        public int licz_z ;
        public PCB odb_z;
        public int grupa = 0;
        public void cpu_stan_zapisz()
        {
            zap_Z = Proc.zap;
            zap2_Z = Proc.zap2;
            przywroc_z = Proc.przywroc;
            przywroc1_z = Proc.przywroc1;
            zap3_z = Proc.zap3;
            licz_z = Proc.licz;
            odb_z = Proc.odb;
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
            Console.WriteLine("Zapisano rejestry do PCB.");

        }

        public void cpu_stan_laduj()
        {
            Proc.zap=zap_Z;
            Proc.zap2=zap2_Z ;
            Proc.przywroc=przywroc_z;
            Proc.przywroc1=przywroc1_z;
             Proc.zap3=zap3_z ;
             Proc.licz=licz_z ;
             Proc.odb =odb_z;
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
            Console.WriteLine("ZAladowano rejestry z PCB.");
        }

        public PCB(string name)
        {
            zap_Z = null;
            zap2_Z = null;
            przywroc_z = false;
            przywroc1_z = false;
            odb_z = null;
            zap3_z = null;
            licz_z = 0;
            NAME = name;
            BLOCKED = false;
            STOPPED = false;
            NEXT_PCB_ALL = null;
            LAST_PCB_ALL = null;
            NEXT_PCB_GROUP = null;
            LAST_PCB_GROUP = null;
            FIRST_MESSAGE = new Queue<MESSAGE>();
            MESSAGE_SEMAPHORE_COMMON=new SEMAPHORE(1,"COMMON");
            MESSAGE_SEMAPHORE_RECEIVER=new SEMAPHORE(0,"RECEIVER");
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
        public Queue<PCB> semaphoreList = new Queue<PCB>();
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
                S.semaphoreList.Enqueue(zawiadowca.RUNNING);
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
                PCB t = S.semaphoreList.Dequeue();
                t.BLOCKED = false;
                if (zawiadowca.NEXTTRY_MODIFIED == false)
                {
                    zawiadowca.NEXTTRY = t;
                    zawiadowca.NEXTTRY_MODIFIED = true;
                }
                return;

            }
            return;
        }
    }


}


