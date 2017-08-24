using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BCM2835
{
    public static class Native
    {
        [DllImport("libc", SetLastError = true)]
        public static extern int geteuid();

        [DllImport("libc", SetLastError = true, EntryPoint = "fopen")]
        private static extern int _fopen(IntPtr File, IntPtr Mode);

        [DllImport("libc", SetLastError = true)]
        private static extern IntPtr malloc(UInt32 size);

        [DllImport("libc", SetLastError = true)]
        private static extern void free(IntPtr mem);

        [DllImport("libc", SetLastError = true)]
        public static extern int close(int fd);

        [DllImport("libc", SetLastError = true, EntryPoint = "open")]
        private static extern int _open(IntPtr PathName, int Flags);

        [DllImport("libc", SetLastError = true)]
        public static extern IntPtr mmap(IntPtr address, uint size, MMapProt prot, MMapFlags flags, int fd, int offset);

        [DllImport("libc", SetLastError = true)]
        public static extern int munmap(IntPtr address, uint length);
        
        public static int fopen(string File, string Mode)
        {

            IntPtr file = MallocString(File);
            IntPtr mode = MallocString(Mode);

            int res = _fopen(file, mode);

            FreeMallocString(file);
            FreeMallocString(mode);

            return res;

        }

        public static int open(string Path, OpenFlags Flags)
        {
            int flags = (int)Flags;
            IntPtr path = MallocString(Path);
            int result = _open(path, flags);
            FreeMallocString(path);
            return result;
        }

        private static unsafe IntPtr MallocString(string ToMalloc)
        {
            byte[] data = Encoding.UTF8.GetBytes(ToMalloc);

            IntPtr encData = malloc((uint)data.Length + 4);

            byte* ef = (byte*)encData.ToPointer();

            for (int buc = 0; buc < data.Length; buc++)
                ef[buc] = data[buc];

            ef[data.Length] = ef[data.Length + 1] = ef[data.Length + 2] = ef[data.Length + 3] = 0;

            return encData;
        }

        private static void FreeMallocString(IntPtr String)
        {
            free(String);
        }

        public enum OpenFlags : Int32
        {
            O_ACCMODE = 3,
            O_RDONLY = 0,
            O_WRONLY = 1,
            O_RDWR = 2,
            O_CREAT = 64,
            O_EXCL = 128,
            O_NOCTTY = 256,
            O_TRUNC = 512,
            O_APPEND = 1024,
            O_NONBLOCK = 2048,
            O_NDELAY = 2048,
            O_SYNC = 1052672,
            O_FSYNC = 1052672,
            O_ASYNC = 8192,

        }
        
        public enum MMapProt : UInt32
        {
            PROT_NONE = 0,
            PROT_READ = 1,
            PROT_WRITE = 2,
            PROT_EXEC = 4,
            
        }

        public enum MMapFlags : UInt32
        {
            MAP_FILE = 0,
            MAP_SHARED = 1,
            MAP_PRIVATE = 2,
            MAP_FIXED = 16,
            MAP_ANONYMOUS = 32,
            MAP_GROWSDOWN = 256,
            MAP_DENYWRITE = 2048,
            MAP_EXECUTABLE = 4096,
            MAP_LOCKED = 8192,
            MAP_NORESERVE = 16384,
            MAP_POPULATE = 32768,
            MAP_NONBLOCK = 65536,
            MAP_STACK = 131072,
            MAP_HUGETLB = 262144
        }
    }
}
