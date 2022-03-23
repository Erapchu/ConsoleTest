using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TestLibrary;

namespace ConsoleTest
{
    class Program
    {
        private const string SpecializedSymbolSequence = "XXX";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PasteClipboardDataToActiveWindow();
            //GetResourceStringDictionary();
            //GetIpAddress();
            //BoxingTest();
            //DnsTest();
            //AssemblyTest();
            //DirectoryInfoTest("Z:\\");
            //TryParseChecks();
            //OverflowChecks();
            //LookupTest();
            //TestFinallyThrow();
            //TestConcurrentDictionary();
            //ComposeUri(@"\\Andrew_k_j@hotmail.com\Inbox");
            //ComposeUri(@"\\outlook data file\Inbox");
            //MemoryUsingStrings();
            //UriLeftPartBenchmark();
            //CheckGuids();
            //VirtualCOMObjects();
            //NullOperations();
            //TestPaths();

            Console.ReadKey();
        }

        private static void PasteClipboardDataToActiveWindow()
        {
            string inputData = null;
            while (string.IsNullOrWhiteSpace(inputData))
            {
                Console.Write("Input data: ");
                inputData = Console.ReadLine();
            }

            var hglobal = Marshal.StringToHGlobalUni(inputData);
            OpenClipboard(IntPtr.Zero);
            EmptyClipboard();
            SetClipboardData(CF_UNICODETEXT, hglobal);
            Marshal.FreeHGlobal(hglobal);
            CloseClipboard();

            System.Threading.Thread.Sleep(1000);
            keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
            keybd_event(VK_V, 0, 0, UIntPtr.Zero);
            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public const byte VK_V = 0x56;
        public const byte VK_CONTROL = 0x11;
        public const int KEYEVENTF_KEYUP = 0x0002;

        public const uint WM_PASTE = 0x0302;
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint CF_UNICODETEXT = 0xD; // 13
        public const uint CF_TEXT = 0x1; // 1

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread
        /// that created the specified window and returns without waiting for the
        /// thread to process the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="Msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseClipboard();

        private static void GetResourceStringDictionary()
        {
            var en = Resources.String1;
            Console.WriteLine(en);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
            var ru = Resources.String1;
            Console.WriteLine(ru);
        }

        private static void GetIpAddress()
        {
            string localIP;
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }

        private static void BoxingTest()
        {
            Console.ReadKey();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000000; i++)
            {
                i.GetHashCode();
            }
            sw.Stop();

            Console.WriteLine(sw.Elapsed);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100000000; i++)
            {
                HashCode.Combine(i);
            }
            sw.Stop();

            Console.WriteLine(sw.Elapsed);

            //Combine(1);
            //Combine(new object());
        }

        public static int Combine<T1>(T1 value1)
        {
            uint hc1 = (uint)(value1?.GetHashCode() ?? 0);
            hc1 = (uint)(value1 != null ? value1.GetHashCode() : 0);
            if (value1 != null)
            {
                hc1 = (uint)value1.GetHashCode();
            }

            var isNull = value1 == null;
            if (isNull)
            {
                hc1 = 0;
            }
            else
            {
                hc1 = (uint)value1.GetHashCode();
            }

            //uint hash = MixEmptyState();
            //hash += 4;

            //hash = QueueRound(hash, hc1);

            //hash = MixFinal(hash);
            return 0;
        }

        private static void DnsTest()
        {
            var uncPath = UNCPathHelper.GetConnectionPath("Z:\\");
            Console.WriteLine($"UNC path: \"{uncPath}\"");
            var hostName = uncPath.Split('\\', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            Console.WriteLine($"Host name: {hostName}");

            var hostEntry = Dns.GetHostEntry(hostName);
            foreach (var address in hostEntry.AddressList)
            {
                Console.WriteLine($"IP: {address}");
            }
        }

        private static void AssemblyTest()
        {
            var knownImplementedTypes = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .SelectMany(a => Assembly.Load(a).GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(IMagic)) && !t.IsAbstract && !t.IsInterface));
            var classNames = string.Join(", ", knownImplementedTypes.Select(t => t.Name));
            Console.WriteLine(classNames);
        }

        private static void DirectoryInfoTest(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            var exists = dirInfo.Exists;
        }

        private static void TryParseChecks()
        {
            var nullableParsed = long.TryParse(null, out long nullableLong);
            var emptyParsed = long.TryParse(string.Empty, out long emptyLong);
        }

        private static void OverflowChecks()
        {
            var rand = new Random();
            int test = 0;

            //obscured enough that the compiler doesn't "know" that the line will produce an overflow
            //Does not run explicitly as checked, so no runtime OverflowException is thrown
            test = rand.Next(Int32.MaxValue - 2, Int32.MaxValue) + 10;

            //simple enough that the compiler "knows" that the line will produce an overflow
            //Compilation error (line 16, col 10): The operation overflows at compile time in checked mode
            //test = Int32.MaxValue + 1;

            //Explicitly running as unchecked. Compiler allows line that is "known" to overflow.
            unchecked
            {
                test = Int32.MaxValue + 1;
            }

            Console.WriteLine(test);

            //Explicitly running as unchecked. Still no runtime OverflowException
            unchecked
            {
                test = test - 10;
            }

            Console.WriteLine(test);

            //Explicitly running as checked. System.OverflowException: Arithmetic operation resulted in an overflow.
            checked
            {
                test = test + 10;
            }

            Console.WriteLine(test);
        }

        private static void LookupTest()
        {
            // Just types covering some different assemblies
            Type[] sampleTypes = new[]
            {
                typeof(List<>),
                typeof(string),
                typeof(Enumerable)
            };

            // All the types in those assemblies
            IEnumerable<Type> allTypes = sampleTypes.Select(t => t.Assembly).SelectMany(a => a.GetTypes());

            // Grouped by namespace, but indexable
            ILookup<string, Type> lookup = allTypes.ToLookup(t => t.Namespace);

            foreach (Type type in lookup["System"])
            {
                Console.WriteLine("{0}: {1}", type.FullName, type.Assembly.GetName().Name);
            }
        }

        private static void TestFinallyThrow()
        {
            try
            {
                try
                {
                    throw new Exception("exception thrown from try block");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inner catch block handling {0}.", ex.Message);
                    throw;
                }
                finally
                {
                    Console.WriteLine("Inner finally block");
                    throw new Exception("exception thrown from finally block");
                    Console.WriteLine("This line is never reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer catch block handling {0}.", ex.Message);
            }
            finally
            {
                Console.WriteLine("Outer finally block");
            }
        }

        private static void TestConcurrentDictionary()
        {
            var dict = new ConcurrentDictionary<MyClass, MyClass>();
            var item1 = new MyClass();
            var resultItem1 = dict.GetOrAdd(item1, item1);

            var item2 = new MyClass();
            var resultItem2 = dict.GetOrAdd(item2, item2);

            var eq = Equals(item1, resultItem2);
            var refEq = ReferenceEquals(item1, resultItem2);

            //Assert.Same(item1, resultItem2);
        }

        private static void ComposeUri(string path)
        {
            const string scheme = "outlook";

            path = path.TrimStart('\\').Replace('\\', '/');
            var testString = $"{scheme}:///{path}";

            try
            {
                var uri = new Uri(testString, UriKind.RelativeOrAbsolute);

                var properties = uri.GetType().GetProperties();
                foreach (var uriProperty in properties)
                {
                    Console.WriteLine($"{uriProperty.Name} = {uriProperty.GetValue(uri)}");
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{testString}:\r\n{ex}");
            }
        }

        private static void MemoryUsingStrings()
        {
            var strings = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                strings.Add("00000000234275EDB5CDE74EB9AD0D47DCE16D400700DDB2F87163780843ADFEFD5D7910BCD50004F1E8063B0000DDB2F87163780843ADFEFD5D7910BCD50004F1E812B20000");
            }
            // +10000 string in array ~120 KB
        }

        private static void UriLeftPartBenchmark()
        {
            var uriArray = new List<Uri>();
            for (int i = 0; i < 100000; i++)
            {
                uriArray.Add(new Uri("https://irradiant.sharepoint.com/sites/Dev/Shared%20Documents/Docs/emails?parentWeb=/sites/Dev&amp;folderType=Common"));
            }

            var concatenated = new List<string>();
            var sw = Stopwatch.StartNew();
            foreach (var uriInstance in uriArray)
            {
                concatenated.Add(uriInstance.Host + uriInstance.LocalPath);
            }
            sw.Stop();
            Console.WriteLine($"Concatenated:\t{sw.Elapsed}");

            sw.Reset();
            var generated = new List<string>();
            sw.Start();
            foreach (var uriInstance in uriArray)
            {
                generated.Add(uriInstance.GetLeftPart(UriPartial.Path));
            }
            sw.Stop();
            Console.WriteLine($"Generated:\t{sw.Elapsed}");
        }

        private static void CheckGuids()
        {
            // Check how fast Guid works
            var list = new List<Foo>();
            for (int i = 0; i < 2000; i++)
            {
                list.Add(new Foo(Guid.NewGuid()));
            }
        }

        private static void VirtualCOMObjects()
        {
            // Obtain new first COM object
            Foo someOtherFoo = new Foo("B");
            // Save reference to COM object into variable
            Foo foo = someOtherFoo;
            // Rewrite new second COM object to the same first variable
            someOtherFoo = new Foo("C");
            // Then, someOtherFoo - 1st COM, foo - 2nd COM, NOT the same
        }

        private static void NullOperations()
        {
            // C# netcoreapp3.1
            int? a = 10;
            int? b = null;
            a -= b;
        }

        private static void TestPaths()
        {
            string input;
            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                Console.WriteLine("Indexing path: " + PathTransformer.GetIndexingPath(input));
                Console.WriteLine("Searching path: " + PathTransformer.GetSearchingPath(input));
                Console.WriteLine("Splitted path: " + PathTransformer.GetSplittedPath(input));
                Console.WriteLine();
            }
        }

        class Foo
        {
            public string SomeProperty { get; private set; }
            public Guid Proper { get; }
            public Foo(string bar) { SomeProperty = bar; }
            public Foo(Guid guid) { Proper = guid; }
        }

        public static void CheckStaleIndices()
        {
            var indexedList = new List<string>()
            {
                @"C:\",
                @"C:\Windows",
                @"C:\Program"
            };

            var fetchedList = new List<string>()
            {
                @"C:\",
                @"C:\Program",
                @"C:\Info"
            };

            var newIndices = fetchedList.Except(indexedList);
            var staleIndices = indexedList.Except(fetchedList);
        }
    }
}
