using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsServices.Memory
{
    internal struct ExternMemoryBasicInformation
    {
        public int BaseAddress;
        public int AllocationBase;
        public int AllocationProtect;
        public int RegionSize;   // size of the region allocated by the program
        public int State;   // check if allocated (MEM_COMMIT)
        public int Protect; // page protection (must be PAGE_READWRITE)
        public int lType;
    }
}
