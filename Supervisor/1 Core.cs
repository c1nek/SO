using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processor
{
    public static class zawiadowca
    {
        public PCB RUNNING=null;
        public PCB NEXTTRY=null;
        public bool NEXTTRY_MODIFIED=false;

    }


    public class PCB
    {
        /*PCB*/
        public string nazwaProcesu;
        public bool running;
        public bool stopped;
        public bool blocked;
        public bool semafor_info;
        public int group_indeks;
        public int instruction_done;
        public bool czy_sprawdzony;
        public string[] MEMORY_BLOCK;
        public PCB NEXT_PCB_ALL;
        public PCB LAST_PCB_ALL;
        public PCB NEXT_PCB_GROUP;
        public PCB LAST_PCB_GROUP;
        public object[] cpu_stan = new object[5];

        public void cpu_stan_zapisz()
        {
            cpu_stan[0] = rejestry.get_r0();
            cpu_stan[1] = rejestry.get_r1();
            cpu_stan[2] = rejestry.get_r2();
            cpu_stan[3] = rejestry.get_r3();
            cpu_stan[4] = rejestry.get_lr();
        }

        public PCB(string name, int time, int group)
        {
            
            nazwaProcesu = name;
            running = false;
            blocked = false;
            stopped = false;
            semafor_info = false;
            group_indeks = group;
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
        private static object r0; //akumulator
        private static object r1;
        private static object r2;
        private static object r3;
        private static object lr;

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



    public class Semafor
    {
        private int wartosc;
        public List<PCB> waiting_proceses = new List<PCB>();

        public Semafor()
        {
            wartosc = 0;
        }

        public void p(PCB x)
        {
            Console.WriteLine("Wykonuje program P semafora");

            if (wartosc > 0)
            {
                Console.WriteLine("P: Przyznaje dostep do semafora");
                x.semafor_info = true;
            }
            else
            {

                Console.WriteLine(" P: Brak dostepu - dodaje proces na liste oczekujacych");
                waiting_proceses.Add(x);
            }
            wartosc -= 1;
        }

        public void v()
        {
            Console.WriteLine("Wykonuje program V semafora");
            wartosc += 1;
            if (waiting_proceses.Count() > 0)
            {
                Console.WriteLine("V: Przyznaje dostep procesowi z listy oczekujacych");
                semafor_waiting();
            }
            else
            {
                Console.WriteLine("V: Lista oczekujacych procesow pod semaforem jest pusta");
            }
        }

        public void semafor_waiting()
        {
            PCB x = waiting_proceses[0];
            waiting_proceses.RemoveAt(0);
        }



    }


}


