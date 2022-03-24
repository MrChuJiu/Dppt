using System;
using System.Runtime.InteropServices;

namespace Easy.Core.LinuxSo
{
    internal class Program
    {
        static void Main(string[] args)
        {
              





           Console.WriteLine("Hello World!");
        }
    }

    public class SoTester
    {
        private const string LibraryName = "libzmq";

        const int RTLD_NOW = 2; // for dlopen's flags
        const int RTLD_GLOBAL = 8;

        [DllImport(@"libdl.so.2")]
        public static extern IntPtr dlopen(string filename, int flags);
        [DllImport("libdl.so.2")]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("libdl.so.2", EntryPoint = "dlopen")]
        private static extern IntPtr UnixLoadLibrary(String fileName, int flags);

        [DllImport("libdl.so.2", EntryPoint = "dlclose", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int UnixFreeLibrary(IntPtr handle);

        [DllImport("libdl.so.2", EntryPoint = "dlsym", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr UnixGetProcAddress(IntPtr handle, String symbol);

        [DllImport("libdl.so.2", EntryPoint = "dlerror", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr UnixGetLastError();

        public delegate int sumHandler(int a, int b);
        public static sumHandler sumfunc = null;

        [DllImport("libNativeLib.so", EntryPoint = "sum", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Sum(int a, int b);

        public void Start()
        {
            IntPtr libPtr = IntPtr.Zero;

            string libName = $"{AppContext.BaseDirectory}libNativeLib.so";

            libPtr = UnixLoadLibrary(libName, 2 | 8);

            //libPtr = dlopen(libName, RTLD_NOW);

            if (libPtr != IntPtr.Zero)
                Console.WriteLine($"调用dlopen打开{libName}成功");
            else
                Console.WriteLine($"调用dlopen打开{libName}失败");

            var sumPtr = UnixGetProcAddress(libPtr, "sum");

            if (sumPtr != IntPtr.Zero)
                Console.WriteLine($"dlopen调用sum成功");
            else
                Console.WriteLine($"dlopen调用sum失败");

            sumfunc = Marshal.GetDelegateForFunctionPointer<sumHandler>(sumPtr);

            int ret = sumfunc(1, 3);

            Console.WriteLine($"调用sum结果:{ret}");

            var sumRet = Sum(5, 7);

            Console.WriteLine($"DllImport调用sum结果:{sumRet}");

            //var libname2 = $"libc.so.6";
            var libname2 = $"{AppContext.BaseDirectory}libzmq.so";
            //var libname2 = $"{AppContext.BaseDirectory}libAdminConsole.so";
            var consolePtr = UnixLoadLibrary(libname2, 2 | 8);
            var erroPtr = UnixGetLastError();
            Console.WriteLine($"错误描述：{Marshal.PtrToStringAnsi(erroPtr)}");

            if (consolePtr != IntPtr.Zero)
                Console.WriteLine($"打开{libname2}成功");
            else
                Console.WriteLine($"打开{libname2}失败");
        }
    }
}
