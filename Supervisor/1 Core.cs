using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processor
{
    class Core
    {

    }
    class PCB
    {
        int PCB_LIST_ALL_POINTER;
    }
  public static class Hw
    {
        static object Reg1=null;
        static object Reg2=null;
        List<PCB> PCB_LIST_ALL=new List<PCB>();
        
        public static byte[] MEM = new byte[100];
    }
}
