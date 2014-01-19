using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            lista a=new lista();
            lista b = new lista();
            lista c = new lista();
            object wsk;
            object tmp;
            lista tmpl;
            wsk = a;
            wsk.a = 1;
            wsk.next = b;
            wsk.next.a = 2;
            wsk.next.next = c;
            wsk.next.next.a = 3;
            System.Console.WriteLine("{0}, {1}, {2}\n", a.a, b.b, c.c);
            System.Console.WriteLine("{0}, {1}, {2}\n", wsk.a, wsk.next.a, wsk.next.next.a);
 
        }
    }
    class lista
    {
        int a = 0;
        object next = null;
    }
}
