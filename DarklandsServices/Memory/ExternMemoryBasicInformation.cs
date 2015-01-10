namespace DarklandsServices.Memory
{
    internal struct ExternMemoryBasicInformation
    {
        // disable reshaper warnings - stuct used in native code must remain as they are
#pragma warning disable 649
#pragma warning disable 169
        public int BaseAddress;
        public int AllocationBase;
        public int AllocationProtect;
        public int RegionSize;   // size of the region allocated by the program
        public int State;   // check if allocated (MEM_COMMIT)
        public int Protect; // page protection (must be PAGE_READWRITE)
        public int Type;
#pragma warning restore 169
#pragma warning restore 649
    }
}
