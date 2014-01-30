using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor;
using Process;
using Memory;
using External;
using Interpreter;

namespace Supervisor
{
    public static class IBSUP
    {
        public enum rozkaz : byte { SVC, MOV, ADD, SUB, MUL, DIV, INC, DEC, JUMPF, JUMPR, JZ, JMP, METHOD, FLAG, POWROT, KONIEC, JUMPV };
        public enum wartosc_SVC : byte { P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q };
        public enum wartosc_TYP : byte { R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, LR, MEM, WART, SEM, PROG };
        public enum wartosc_SEM : byte { MEMORY, COMMON, RECEIVER, R2_COMMON, R2_RECEIVER, FSBSEM };
        public enum wartosc_METHOD : byte { CZYSC_PODR, PRZYG_XR, INTER_KOM, SPRAWDZENIE, CZYTNIK, SCAN, PRZESZUKAJ_LISTE, PODRECZNA, READ_MSG, INTER_LOAD, PRINT_MSG, EXPUNGE1, EXPUNGE2, EXPUNGE3, EXPUNGE4, WART_MEMORY, POCZATEK_MEM, KONIEC_MEM, GRUPA };
        public enum Eprog : byte { IBSUP, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER, EXPUNGE };
        //Pamięć wstępna. Z niej ładowane do pamięci głównej
        private static byte[] mem = new byte[]{
        
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.E,

                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R3,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('*'),
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('I'),
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('N'),
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.R3,
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,         (byte)wartosc_TYP.PROG,(byte)Eprog.IN,                                     //Uruchomienie procesu *IN

 
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R3,               
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('*'),                      
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                           
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('O'),                      
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                           
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('U'),                      
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                           
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('T'),
                    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                           
                    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,       (byte)wartosc_TYP.R3,               
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,                                            
                    (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,         (byte)wartosc_TYP.PROG,(byte)Eprog.OUT,                                    //Uruchomienie procesu *OUT

                    /////////////////////////////////////////////////////////////////////////////////////////////////////
                    (byte)rozkaz.FLAG, 0,///////////////////////////////////////////////////////////////////FLAGA 0
                    /////////////////////////////////////////////////////////////////////////////////////////////////

        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R3,                                   //przepisanie adresu pamięci roboczej do rejestru 1
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('*'),             //wpisanie znaku do komórki pamięci w rejestrze 1
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                                               //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('I'),             //wpisanie znaku do komórki pamięci w rejestrze 1
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,                                                               //zwiększenie wartości (adresu) w rejestrze 1 o jeden
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('N'),             //wpisanie znaku do komórki pamięci w rejestrze 1
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,       
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.WART,0,8,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      8,                                                      //określenie długości komunikatu
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('R'),             //zapisanie komunikatu
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('E'),
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('A'),
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,Convert.ToByte('D'),
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,0,//
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,0,
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.WART,0,
        /**/    (byte)rozkaz.INC,       (byte)wartosc_TYP.R1,
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,       (byte)wartosc_TYP.R3,
                (byte)rozkaz.ADD,       (byte)wartosc_TYP.WART,     32,
        /**/    (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,      (byte)wartosc_TYP.R0,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,                                                                //wysłanie komunikatu wskazywanego przez reg 2 oczekiwanie na karte job pod adresem 32 pamięci roboczej

                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,       (byte)wartosc_TYP.R0,                                   //wpisanie adresu miejsca komunikatu od IN/OUT
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R4,       (byte)wartosc_TYP.R0,

                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.PRZYG_XR,  (byte)wartosc_TYP.R2, (byte)wartosc_TYP.WART, 2,    //R2 wskazuje na początek pamięci podręcznej, pole na komunikat = 2 bajty
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.R,                                                                //czekanie na komunikat (OK|NO)
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.INTER_KOM,                                                     //ustawia rejestry pod utworzenie procesu uzytkownika

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                (byte)rozkaz.JZ,        (byte)wartosc_TYP.WART, 0, ////////////////////////SKOK do flagi 0
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////SCAN//////SCAN//////SCAN//////SCAN//////SCAN//////SCAN//////SCAN//////SCAN//////SCAN///////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////

                (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,                                                                //stworzenie PCB USERPROG
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.SCAN,                                                          //pobranie pola o wielkosci i jego interpretacja jeżeli jest nie poprawne EXPUNGE
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,           (byte)wartosc_TYP.R6,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,            (byte)wartosc_TYP.PROG,    (byte)Eprog.IN,
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,           (byte)wartosc_TYP.R7,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.C,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,             (byte)wartosc_TYP.PROG,    (byte)Eprog.IN,

                (byte)rozkaz.JZ,        (byte)wartosc_TYP.PROG,         (byte)Eprog.EXPUNGE,

                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                /////LOAD///////LOAD////LOAD////////LOAD/////LOAD////LOAD//////LOAD//////LOAD////LOAD/////LOAD///////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////////

                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R1,           (byte)wartosc_TYP.R5,
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R0,           (byte)wartosc_TYP.MEM,
                (byte)rozkaz.MUL,       (byte)wartosc_TYP.WART,         8,
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.MEM,          (byte)wartosc_TYP.R0,
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R2,           (byte)wartosc_TYP.R1,   //pomnożenie rozmiaru z kary job razy 8
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R9,           (byte)wartosc_TYP.R0,   //zapamiętanie wielkosci pamieci
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.A,                                    //Przydzielenie pamięci na program użytkownika
                (byte)rozkaz.INC,       (byte)wartosc_TYP.R2,
                (byte)rozkaz.INC,       (byte)wartosc_TYP.R2,                                   
                (byte)rozkaz.MOV,       (byte)wartosc_TYP.R8,           (byte)wartosc_TYP.R2,   //zapamiętanie adresu pamięci użytkownika

                //////////////////////////////////////////////////////////////////////////////////////////
                (byte)rozkaz.FLAG,      1,//////////////////////////////////////////////////FLAGA 1
                //////////////////////////////////////////////////////////////////////////////////////////

                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.READ_MSG, 
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.PRZYG_XR,  (byte)wartosc_TYP.R3, (byte)wartosc_TYP.WART, 2,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.R,
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.INTER_LOAD,
                (byte)rozkaz.JZ,        (byte)wartosc_TYP.WART, 1,


                //////////////////////////////////////////////////////////////////////////////////////////
                //////ENDCARD//////ENDCARD//////ENDCARD//////ENDCARD//////ENDCARD//////ENDCARD//////ENDCARD
                //////////////////////////////////////////////////////////////////////////////////////////

                (byte)rozkaz.SVC,       (byte)wartosc_SVC.Y,            (byte)wartosc_TYP.R8,
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.PRZYG_XR,  (byte)wartosc_TYP.R3, (byte)wartosc_TYP.WART, 2,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.R,
                (byte)rozkaz.METHOD,    (byte)wartosc_METHOD.PRINT_MSG,
                (byte)rozkaz.SVC,       (byte)wartosc_SVC.S,

                (byte)rozkaz.JZ,        (byte)wartosc_TYP.PROG,         (byte)Eprog.EXPUNGE,

        };


        private static byte[] EXPUNGE = new byte[]{

            (byte)rozkaz.METHOD,       (byte)wartosc_METHOD.EXPUNGE1,//pobranie running

            (byte)rozkaz.FLAG,          2,

            (byte)rozkaz.METHOD,        (byte)wartosc_METHOD.EXPUNGE2,//sprawdzenie czy nazwa zaczyna się od * jeżeli nie to wpisuje nazwę do pamięci podr i ustawia na nia r2 i r0=1 else r0=0
            (byte)rozkaz.JUMPF,         (byte)wartosc_TYP.R0,   4,
            (byte)rozkaz.SVC,           (byte)wartosc_SVC.Z,
            (byte)rozkaz.SVC,           (byte)wartosc_SVC.D,
            (byte)rozkaz.METHOD,        (byte)wartosc_METHOD.EXPUNGE3,//pobiera następny blok i sprawdza czy running wskazuje na ten blok r0=0 jezeli wskazuje, r0=1 jezeli nie
            (byte)rozkaz.JZ,            (byte)wartosc_TYP.WART, 2,
            (byte)rozkaz.METHOD,        (byte)wartosc_METHOD.EXPUNGE4,//sprawdza czy rejestry r8 i r9 zawierają wartości i jeżeli tak to przygotowuje pamięć podręczną i r0=1 inaczej r0=0
            (byte)rozkaz.JZ,            (byte)wartosc_TYP.WART, 0,
            (byte)rozkaz.SVC,           (byte)wartosc_SVC.F,
            
            (byte)rozkaz.JMP,           (byte)wartosc_TYP.WART, 0//Powrót z pogramu EXPUNGE
        };


        public static void EXPUNGE1()
        {
            rejestry.r4 = zawiadowca.RUNNING;
        }

        public static void EXPUNGE2()
        {
            if (((PCB)rejestry.r4).NAME[0] == '*')
            {
                rejestry.r0 = 0;
                return;
            }
            else
            {
                int adr = (int)rejestry.r3;
                int j = 0;
                for (; j < ((PCB)rejestry.r4).NAME.Length; adr++)
                {

                    Mem.MEMORY[adr] = Convert.ToByte(((PCB)rejestry.r4).NAME[j]);
                        j++;
                }
                for (;j<8 ; j++)
                {
                    Mem.MEMORY[adr] = 0;
                    adr++;
                }
                rejestry.r2 = rejestry.r3;
                rejestry.r0 = 1;
            }
        }

        public static void EXPUNGE3()
        {
            rejestry.r4 = ((PCB)rejestry.r4).NEXT_PCB_GROUP;
            if (((PCB)rejestry.r4) == zawiadowca.RUNNING)
            {
                rejestry.r0 = 1;
            }
            else
            {
                rejestry.r0 = 0;
            }
        }
        public static void EXPUNGE4()
        {
            if ((int)rejestry.r8 >= 100 && (int)rejestry.r9 >= 8)
            {
                int adr = (int) rejestry.r3;
                rejestry.r2=adr;
                byte[] tmpB = BitConverter.GetBytes((int)rejestry.r9);
                byte[] tmpC = BitConverter.GetBytes((int)rejestry.r8);
                if (BitConverter.IsLittleEndian == true)
                {
                    Mem.MEMORY[adr++] = tmpB[1];
                    Mem.MEMORY[adr++] = tmpB[0];
                    Mem.MEMORY[adr++] = tmpC[1];
                    Mem.MEMORY[adr++] = tmpC[0];
                }
                else
                {
                    Mem.MEMORY[adr++] = tmpB[2];
                    Mem.MEMORY[adr++] = tmpB[3];
                    Mem.MEMORY[adr++] = tmpC[2];
                    Mem.MEMORY[adr++] = tmpC[3];
                }
                rejestry.r0 = 1;
            }
            else
            {
                rejestry.r0 = 0;
            }
        }
        public static int zaladuj(int m)
        {
            int i;
            for (i = 0; i < mem.Length; i++)
            {
                Mem.MEMORY[i+m] = mem[i];
            }
            return m + i + 1;
        }

        public static int zaladujEXPUNGE(int m)
        {
            int i;
            for (i = 0; i < EXPUNGE.Length; i++)
            {
                Mem.MEMORY[i + m] = EXPUNGE[i];
            }
            return m + i + 1;
        }

        public static void czyscPodr(int adr, int rozmiar)
        {
            for (int i = 0; i < rozmiar; i++)
            {
                Mem.MEMORY[adr + i] = 0;
            }
            rejestry.r8 = (Int16)(int)rejestry.r3;//zapisanie roczatku pamięci roboczej do 8 rejestru
        }

        public static void przygXR(int adr, int dl)
        {
            int tmp = adr;
            tmp += 8;
            Mem.MEMORY[tmp] =(byte) dl;
        }

        public static void interKom()
        {
            int i = (int)rejestry.r2;
            int j;
            string nazwa=null;
            string komunikat=null;

            for (; i < (int)rejestry.r2+8; i++)
            {

                if (Mem.MEMORY[i] != 0)
                {
                    nazwa += (char)Mem.MEMORY[i];//???? nie wiem czy działa
                }
            }
            if (nazwa != "*IN")
            {
                System.Console.WriteLine("Blad: zly nadawca. Oczekiwana wartosc to *IN. Otrzymano {0}", nazwa);
                Console.ReadLine();
                Environment.Exit(0);
            }
            int wielkosc = Mem.MEMORY[i];//wielkosc komunikatu
            j = i;
            for (; i < j + wielkosc; i++)
            {
                if (Mem.MEMORY[i] != 0)
                {
                    komunikat += (char)Mem.MEMORY[i];//???? nie wiem czy działa
                }
            }
            if (komunikat != "OK")
            {
                System.Console.WriteLine("Blad: zly komunikat. Oczekiwana wartosc to OK. Otrzymano {0}", nazwa);
                Console.ReadLine();
                Environment.Exit(0);
            }
            i = (int)rejestry.r2 +34;
            if (Mem.MEMORY[i] != '$')
            {
                System.Console.WriteLine("Blad: inna karta. Oczekiwana wartosc to $JOB. Otrzymano {0}{1}{2}{3}", (char)Mem.MEMORY[i], (char)Mem.MEMORY[i+1], (char)Mem.MEMORY[i+2], (char)Mem.MEMORY[i+3]);//domyslnie powinien jeszcze raz czytać
                Console.ReadLine();
                rejestry.r0 = 0;
            }
            else
                rejestry.r0 = 1;
            i += 36;//ustawienie na 70 bajt pamieci podr
            rejestry.r2 = i;
            Mem.MEMORY[i++] = (byte)'U';
            Mem.MEMORY[i++] = (byte)'S';
            Mem.MEMORY[i++] = (byte)'E';
            Mem.MEMORY[i++] = (byte)'R';
            Mem.MEMORY[i++] = (byte)'P';
            Mem.MEMORY[i++] = (byte)'R';
            Mem.MEMORY[i++] = (byte)'O';
            Mem.MEMORY[i++] = (byte)'G';


        }

        public static void SCAN()//zapamiętuje wielkość w rejestrze 5, wskaźnik na nazwę IN w rejestrze 6, wskaźnik na nazwę OUT w rejestrze 7
        {
            int adrPoczatek = (int) rejestry.r4+2;
            int tmp = adrPoczatek;
            int tmp2;
            string tekst;
            tekst = System.Text.Encoding.UTF8.GetString(Mem.MEMORY, tmp, 5);
            tmp += 5;
            if (tekst != "$JOB,")
            {
                Console.WriteLine("Błędna składnia: {0}", tekst);
                rejestry.r0 = 0;
                return;
            }
            if (Convert.ToChar(Mem.MEMORY[tmp + 2]) != ',')
            {
                Console.WriteLine("Błędna składnia $JOB. Znak: {0}", tmp);
                rejestry.r0 = 0;
                return;
            }
            rejestry.r5 = Mem.MEMORY[tmp];
            tmp += 3;
            tmp2=tmp;
            tekst = null;
            for (; Convert.ToChar(Mem.MEMORY[tmp]) != '='; tmp++)
            {
                tekst += Convert.ToChar(Mem.MEMORY[tmp]);
                Console.WriteLine("Nazwa Procesu: {0}", tekst);
            }
            if (Convert.ToChar(Mem.MEMORY[tmp + 1]) == 'I')
            {
                rejestry.r6 = tmp2;
                tmp += 4;
            }
            else if (Convert.ToChar(Mem.MEMORY[tmp + 1]) == 'O')
            {
                rejestry.r7 = tmp2;
                tmp += 5;
            }
            for (; Convert.ToChar(Mem.MEMORY[tmp]) != '='; tmp++)
            {
                tekst += Convert.ToChar(Mem.MEMORY[tmp]);
                Console.WriteLine("Nazwa Procesu: {0}", tekst);
            }
            if (Convert.ToChar(Mem.MEMORY[tmp + 1]) == 'I')
            {
                rejestry.r6 = tmp2;
                
            }
            else if (Convert.ToChar(Mem.MEMORY[tmp + 1]) == 'O')
            {
                rejestry.r7 = tmp2;
                
            }
            rejestry.r0 = 1;

            //schemat 7.9
            //ustawia r0 = 1 gdy wszysko ok
            //ustawia r0 = 0 gdy błąd składni
        }

        public static void READ_MSG()
        {
            int adrPocz = (int)rejestry.r3;
            int tmp = adrPocz;
            int adrKom = adrPocz + 32;
            byte[] tmpB = BitConverter.GetBytes(adrKom);
           

            Mem.MEMORY[tmp++] = Convert.ToByte('*');
            Mem.MEMORY[tmp++] = Convert.ToByte('I');
            Mem.MEMORY[tmp++] = Convert.ToByte('N');
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 8;
            Mem.MEMORY[tmp++] = Convert.ToByte('R');
            Mem.MEMORY[tmp++] = Convert.ToByte('E');
            Mem.MEMORY[tmp++] = Convert.ToByte('A');
            Mem.MEMORY[tmp++] = Convert.ToByte('D');
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            if (BitConverter.IsLittleEndian == true)
            {
                Mem.MEMORY[tmp++] = tmpB[1];
                Mem.MEMORY[tmp++] = tmpB[0];
            }
            else
            {
                Mem.MEMORY[tmp++] = tmpB[2];
                Mem.MEMORY[tmp++] = tmpB[3];
            }
            rejestry.r2 = adrPocz;

        }

        public static void PRINT_MSG()
        {
            int adrPocz = (int)rejestry.r3;
            int adrKom = adrPocz + 9;
            byte[] tmpB = BitConverter.GetBytes(adrKom);

            int tmp = adrPocz+64;
            Mem.MEMORY[tmp++] = Convert.ToByte('*');
            Mem.MEMORY[tmp++] = Convert.ToByte('O');
            Mem.MEMORY[tmp++] = Convert.ToByte('U');
            Mem.MEMORY[tmp++] = Convert.ToByte('T');
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = 8;
            Mem.MEMORY[tmp++] = Convert.ToByte('P');
            Mem.MEMORY[tmp++] = Convert.ToByte('R');
            Mem.MEMORY[tmp++] = Convert.ToByte('I');
            Mem.MEMORY[tmp++] = Convert.ToByte('N');
            Mem.MEMORY[tmp++] = 0;
            Mem.MEMORY[tmp++] = Mem.MEMORY[adrPocz+8];
            if (BitConverter.IsLittleEndian == true)
            {
                Mem.MEMORY[tmp++] = tmpB[1];
                Mem.MEMORY[tmp++] = tmpB[0];
            }
            else
            {
                Mem.MEMORY[tmp++] = tmpB[3];
                Mem.MEMORY[tmp++] = tmpB[4];
            }
            rejestry.r2 = adrPocz+64;
        }

        private static int GadrUser=0;
        public static void INTER_LOAD()
        {
            int adrUser;
            int adrPocz = (int) rejestry.r3;
            adrUser = (int) rejestry.r8;
            if (GadrUser==0)//ma sie wykonac tylko raz
            {
                GadrUser=adrUser;
            }
            int tmp = adrPocz;
            int prog = adrPocz + 32;
            int dl = 0;
            if (Convert.ToChar(Mem.MEMORY[tmp + 9]) == 'O' && Convert.ToChar(Mem.MEMORY[tmp + 10]) == 'K')
            {
                dl = Mem.MEMORY[prog]<<8;
                dl += Mem.MEMORY[prog + 1];
                prog += 2;
                if (Mem.MEMORY[prog] == (byte)rozkaz.KONIEC)
                {
                    rejestry.r0 = 1;
                    rejestry.r2 = rejestry.r3;
                    int tmp2 = (int) rejestry.r2;
                    Mem.MEMORY[tmp2++] = Convert.ToByte('U');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('S');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('E');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('R');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('P');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('R');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('O');
                    Mem.MEMORY[tmp2++] = Convert.ToByte('G');

                }
                else
                {
                    for (int i = 0; i < dl; i++)
                    {
                        Mem.MEMORY[GadrUser++] = Mem.MEMORY[prog++];
                    }
                    rejestry.r0 = 0;
                }
                
            }
            else
            {
                string sTmp=Convert.ToString(Convert.ToChar(Mem.MEMORY[tmp + 9]));
                sTmp += Convert.ToString(Convert.ToChar(Mem.MEMORY[tmp + 10]));
                Console.WriteLine("Otrzymano błędny komunikat podczas ładowania programu USERPROG. Kod: {0}", sTmp);
            }
        }

        public static void ZERUJ_PAM()
        {

        }
    }

    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

   static public class IPLRTN
   {
       public enum Eprog : byte { IBSUP, EXPUNGE, IN, OUT = 1, P, V, G, A, E, F, B, C, D, H, I, J, N, R, S, Y, Z, Q, USER };
       public static int[] adrProg = new int[25];//adresy początku programów (SVC) nie wszystkie

       public static void CWrite(ConsoleColor color, string text)
       {
           ConsoleColor originalColor = Console.ForegroundColor;
           Console.ForegroundColor = color;
           Console.Write(text);
           Console.ForegroundColor = originalColor;
       }

       public static void Main()
       {

           Console.WindowHeight = 50;
           Console.WindowWidth = 100;
           Console.BufferWidth = 100;
           Console.BufferHeight = 50;
           Console.WindowTop = 0;
           Console.WindowLeft = 0;

           //tworzy swój PCB dodaje go na listę ustaiwa wszystkie wartości by wskazywały na niego
           PCB iplrtn = new PCB("*IPRTLN");
           rejestry.r2 = iplrtn;
           zawiadowca.RUNNING = iplrtn;
           zawiadowca.NEXTTRY = iplrtn;
           iplrtn.LAST_PCB_ALL = iplrtn;
           iplrtn.LAST_PCB_GROUP = iplrtn;
           iplrtn.NEXT_PCB_ALL = iplrtn;
           iplrtn.NEXT_PCB_GROUP = iplrtn;
           iplrtn.STOPPED = true;

           Console.Write("Start programu"); CWrite(ConsoleColor.Cyan, " IPLRTN\n");
           Console.ReadLine();
           Console.WriteLine("Wczytywanie jądra systemu do pamięci");
           Console.ReadLine();

           int i = 0;
           int j = 0;

           adrProg[(int)Eprog.IBSUP] = i;
           i = IBSUP.zaladuj(0);
           

           adrProg[(int)Eprog.EXPUNGE] = i;
           i = IBSUP.zaladujEXPUNGE(i);
           CWrite(ConsoleColor.Cyan, "IBSUB");
           Console.Write(" - wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.IN] = i;
           i = Ext.zaladuj(i);
           CWrite(ConsoleColor.Cyan, "EXT ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.A] = i;
           i = Mem.zaladujXA(i);
           CWrite(ConsoleColor.Cyan, "XA ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.F] = i;
           i = Mem.zaladujXF(i);
           CWrite(ConsoleColor.Cyan, "XF ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.H] = i;
           i = Proc.zaladujXH(i);
           CWrite(ConsoleColor.Cyan, "XH ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.N] = i;
           i = Proc.zaladujXN(i);
           CWrite(ConsoleColor.Cyan, "XN ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.R] = i;
           i = Proc.zaladujXR(i);
           CWrite(ConsoleColor.Cyan, "XR ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.S] = i;
           i = Proc.zaladujXS(i);
           CWrite(ConsoleColor.Cyan, "XS ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.Y] = i;
           i = Proc.zaladujXY(i);
           CWrite(ConsoleColor.Cyan, "XY ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.Z] = i;
           i = Proc.zaladujXZ(i);
           CWrite(ConsoleColor.Cyan, "XZ ");
           Console.Write("- wczytano");
           Console.ReadLine();

           adrProg[(int)Eprog.Q] = i;
           i = Proc.zaladujXQUE(i);
           CWrite(ConsoleColor.Cyan, "XQUE ");
           Console.Write("- wczytano");
           Console.ReadLine();//wpisywanie programów do pamięci głównej i początku każdego z nich do tablicy DODAC EXPUNGE!

           Console.Write("Opisywanie wolnej pamięci przy pomocy bloków FSB");
           if (Mem.start(i) == false) //całą pamięć wolną opisuje przy pomocy bloków FSB 
           {
               Console.Write(" - ");
               CWrite(ConsoleColor.Red, "BŁĄD!");
               Console.ReadLine();
               return;
           }
           else
           {
               Console.Write(" - wykonano");
               Console.ReadLine();
           }//opisywanie wolnej pamięci blaokami FSB

           Console.Write("Tworzenie PCB dla pierwszego strumienia zlecień");
           Console.ReadLine();
           PCB ibsub1 = new PCB("*IBSUB");
           ibsub1.cpu_stan[0] = 0;
           ibsub1.cpu_stan[1] = 0;
           ibsub1.cpu_stan[2] = 0;
           ibsub1.cpu_stan[3] = 0;
           ibsub1.cpu_stan[4] = adrProg[(int)Eprog.IBSUP];
           ibsub1.cpu_stan[6] = 0;
           ibsub1.cpu_stan[7] = 0;
           ibsub1.cpu_stan[8] = 0;
           ibsub1.cpu_stan[9] = 0;
           ibsub1.cpu_stan[10] = 0;
           ibsub1.cpu_stan[11] = 0;


           Console.Write("Tworzenie PCB dla drugiego strumienia zlecień");
           Console.ReadLine();

           PCB ibsub2 = new PCB("*IBSUB");
           ibsub2.cpu_stan[0] = 0;
           ibsub2.cpu_stan[1] = 0;
           ibsub2.cpu_stan[2] = 0;
           ibsub2.cpu_stan[3] = 0;
           ibsub2.cpu_stan[4] = adrProg[(int)Interpreter.Inter.Eprog.IBSUP];
           ibsub1.cpu_stan[6] = 0;
           ibsub1.cpu_stan[7] = 0;
           ibsub1.cpu_stan[8] = 0;
           ibsub1.cpu_stan[9] = 0;
           ibsub1.cpu_stan[10] = 0;
           ibsub1.cpu_stan[11] = 0;

           Console.Write("Ustawianie wskazników we wszystkich PCB");
           Console.ReadLine();
           iplrtn.NEXT_PCB_ALL = ibsub1;

           ibsub1.NEXT_PCB_ALL = ibsub2;
           ibsub2.NEXT_PCB_ALL = iplrtn;

           ibsub1.LAST_PCB_ALL = iplrtn;
           ibsub2.LAST_PCB_ALL = iplrtn;

           ibsub1.NEXT_PCB_GROUP = ibsub1;
           ibsub2.NEXT_PCB_GROUP = ibsub2;

           ibsub1.LAST_PCB_GROUP = ibsub1;
           ibsub2.LAST_PCB_GROUP = ibsub2;

           Console.Write("Ustawienie NEXTTRY i wymuszenie zmiany procesu");
           Console.ReadLine();

           zawiadowca.NEXTTRY = iplrtn.NEXT_PCB_ALL;
           zawiadowca.wymusZmiane = true;

           Console.Write("Uruchomienie");
           CWrite(ConsoleColor.Green, " zawiadowcy\n");
           Console.ReadLine();

           zawiadowca.Run();

       }
   }
}
