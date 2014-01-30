using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Process;
using Processor;
using Memory;
using Interpreter;
using External;


namespace Supervisor
{
    class Format
    {
        public static void CWrite(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            for (int i = 0; i < 20 && Console.CursorLeft != 99; i++)
                Console.Write(' ');
            Console.ForegroundColor = originalColor;
        }

        public static void CWrite(ConsoleColor color, int text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            for(int i =0;i<20&&Console.CursorLeft!=99;i++)
                Console.Write(' ');
            Console.ForegroundColor = originalColor;
        }

        public static void LicznikWrite()
        {
            int kolumnaOrg = Console.CursorLeft;
            int wierszOrg = Console.CursorTop;
            Console.CursorLeft = 80;
            Console.CursorTop = 0;
            Console.Write("LZ: ");
            CWrite(ConsoleColor.Magenta, zawiadowca.licznik);
            Console.CursorLeft = kolumnaOrg;
            Console.CursorTop = wierszOrg;
        }

        public static void RejestyWrite()
        {
            int kolumnaOrg = Console.CursorLeft;
            int wierszOrg = Console.CursorTop;
            Console.CursorLeft = 80;
            Console.CursorTop = 1;
            Console.Write("R0: ");
            if (rejestry.r0 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r0);
            else if (rejestry.r0 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r0).NAME);
            else if (rejestry.r0 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
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
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 3;
            Console.Write("R2: ");
            if (rejestry.r2 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r2);
            else if (rejestry.r2 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r2).NAME);
            else if (rejestry.r2 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 4;
            Console.Write("R3: ");
            if (rejestry.r3 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r3);
            else if (rejestry.r3 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r3).NAME);
            else if (rejestry.r3 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 5;
            Console.Write("R4: ");
            if (rejestry.r4 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r4);
            else if (rejestry.r4 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r4).NAME);
            else if (rejestry.r4 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 6;
            Console.Write("R5: ");
            if (rejestry.r5 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r5);
            else if (rejestry.r5 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r5).NAME);
            else if (rejestry.r5 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 7;
            Console.Write("R6: ");
            if (rejestry.r6 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r6);
            else if (rejestry.r6 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r6).NAME);
            else if (rejestry.r6 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 8;
            Console.Write("R7: ");
            if (rejestry.r7 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r7);
            else if (rejestry.r7 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r7).NAME);
            else if (rejestry.r7 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 9;
            Console.Write("R8: ");
            if (rejestry.r8 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r8);
            else if (rejestry.r8 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r8).NAME);
            else if (rejestry.r8 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 10;
            Console.Write("R9: ");
            if (rejestry.r9 is int)
                CWrite(ConsoleColor.Magenta, (int)rejestry.r9);
            else if (rejestry.r9 is PCB)
                CWrite(ConsoleColor.Magenta, ((PCB)rejestry.r9).NAME);
            else if (rejestry.r9 is SEMAPHORE)
                CWrite(ConsoleColor.Magenta, "SEMAPHORE");
            else
                CWrite(ConsoleColor.Magenta, "inne");
            Console.CursorLeft = 80;
            Console.CursorTop = 11;
            Console.Write("LR: ");
            CWrite(ConsoleColor.Magenta, (int)rejestry.lr);

            Console.CursorLeft = kolumnaOrg;
            Console.CursorTop = wierszOrg;
        }
    }
}
