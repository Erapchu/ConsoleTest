using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UIAutomationClient;

namespace ConsoleTest.WinApi
{
    internal class WinApiProvider
    {
        public static void GetForegroundWindowText()
        {
            while (true)
            {
                try
                {
                    var hwnd = GetForegroundWindow();
                    var text = GetText(hwnd);
                    Console.WriteLine(text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        public static void GetCaretCoordinatesAccessible()
        {
            while (true)
            {
                try
                {
                    var hFore = GetForegroundWindow();
                    var info = new GUITHREADINFO();
                    info.cbSize = Marshal.SizeOf(info);
                    if (GetGUIThreadInfo(0, ref info))
                    {
                        var hwndFocus = info.hwndFocus;
                        var guid = typeof(IAccessible).GUID;
                        object accessibleObject = null;
                        var retVal = AccessibleObjectFromWindow(hwndFocus, OBJID_CARET, ref guid, ref accessibleObject);
                        IAccessible accessible = accessibleObject as IAccessible;
                        accessible.accLocation(out int left, out int top, out int width, out int height, CHILDID_SELF);
                        Console.WriteLine($"Position: l={left} t={top}, w={width}, h={height}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public static void GetCaretCoordinatesWinApi()
        {
            Thread.Sleep(2000);
            var hFore = GetForegroundWindow();
            var idAttach = GetWindowThreadProcessId(hFore, out uint id);
            var curThreadId = GetCurrentThreadId();

            // To attach to current thread
            var sa = AttachThreadInput(idAttach, curThreadId, true);

            var caretPos = GetCaretPos(out POINT caretPoint);

            ClientToScreen(hFore, ref caretPoint);

            // To dettach from current thread
            var sd = AttachThreadInput(idAttach, curThreadId, false);

            var data = string.Format("X={0}, Y={1}", caretPoint.X, caretPoint.Y);
            Console.WriteLine(data);
        }

        public static void GetCaretPosition()
        {
            // needs 'using UIAutomationClient;'
            // to reference UIA, don't use the .NET assembly
            // but instead, reference the UIAutomationClient dll as a COM object
            // and set Embed Interop Types to False for the UIAutomationClient reference in the C# project
            var automation = new CUIAutomation8();
            do
            {
                try
                {
                    if (GetCursorPos(out POINT lpPoint))
                    {
                        var element = automation.ElementFromPoint(new tagPOINT { x = lpPoint.X, y = lpPoint.Y });
                        if (element != null)
                        {
                            Console.WriteLine("Watched element " + element.CurrentName);
                            var guid = typeof(UIAutomationClient.IUIAutomationTextPattern2).GUID;
                            var ptr = element.GetCurrentPatternAs(UIA_PatternIds.UIA_TextPattern2Id, ref guid);
                            if (ptr != IntPtr.Zero)
                            {
                                var pattern = (IUIAutomationTextPattern2)Marshal.GetObjectForIUnknown(ptr);
                                if (pattern != null)
                                {
                                    var documentRange = pattern.DocumentRange;
                                    var caretRange = pattern.GetCaretRange(out _);
                                    if (caretRange != null)
                                    {
                                        var rects = caretRange.GetBoundingRectangles();
                                        Console.WriteLine($"Rects {string.Join(" ", rects.OfType<double>().ToList())}");
                                        var caretPos = caretRange.CompareEndpoints(
                                            UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start,
                                            documentRange,
                                            UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start);
                                        Console.WriteLine(" caret is at " + caretPos);
                                    }
                                }
                            }
                            else
                            {
                                guid = typeof(UIAutomationClient.IUIAutomationTextPattern).GUID;
                                ptr = element.GetCurrentPatternAs(UIA_PatternIds.UIA_TextPatternId, ref guid);
                                if (ptr != IntPtr.Zero)
                                {
                                    var pattern = (IUIAutomationTextPattern)Marshal.GetObjectForIUnknown(ptr);
                                    if (pattern != null)
                                    {
                                        var documentRange = pattern.DocumentRange;
                                        var boundingRect = documentRange.GetBoundingRectangles();
                                        Console.WriteLine($"Rects {string.Join(" ", boundingRect.OfType<double>().ToList())}");
                                        //var caretRange = pattern.GetCaretRange(out _);
                                        //if (caretRange != null)
                                        //{
                                        //    var caretPos = caretRange.CompareEndpoints(
                                        //        UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start,
                                        //        documentRange,
                                        //        UIAutomationClient.TextPatternRangeEndpoint.TextPatternRangeEndpoint_Start);
                                        //    Console.WriteLine(" caret is at " + caretPos);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
            while (true);
        }

        public static void PasteClipboardDataToActiveWindow()
        {
            string inputData = null;
            while (string.IsNullOrWhiteSpace(inputData))
            {
                Console.Write("Input data: ");
                inputData = Console.ReadLine();
            }

            WindowsClipboard.SetText(inputData);

            Thread.Sleep(1000);
            // The following methods are obsolete
            INPUT[] inputs = new INPUT[4];

            inputs[0].type = WindowsKeyboard.INPUT_KEYBOARD;
            inputs[0].U.ki.wVk = WindowsKeyboard.VK_CONTROL;

            inputs[1].type = WindowsKeyboard.INPUT_KEYBOARD;
            inputs[1].U.ki.wVk = WindowsKeyboard.VK_V;

            inputs[2].type = WindowsKeyboard.INPUT_KEYBOARD;
            inputs[2].U.ki.wVk = WindowsKeyboard.VK_V;
            inputs[2].U.ki.dwFlags = WindowsKeyboard.KEYEVENTF_KEYUP;

            inputs[3].type = WindowsKeyboard.INPUT_KEYBOARD;
            inputs[3].U.ki.wVk = WindowsKeyboard.VK_CONTROL;
            inputs[3].U.ki.dwFlags = WindowsKeyboard.KEYEVENTF_KEYUP;

            // Send input simulate Ctrl + V
            var uSent = WindowsKeyboard.SendInput((uint)inputs.Length, inputs, INPUT.Size);

            // Deprecated WinAPI methods
            //WindowsKeyboard.keybd_event(WindowsKeyboard.VK_CONTROL, 0, 0, UIntPtr.Zero);
            //WindowsKeyboard.keybd_event(WindowsKeyboard.VK_V, 0, 0, UIntPtr.Zero);
            //WindowsKeyboard.keybd_event(WindowsKeyboard.VK_V, 0, WindowsKeyboard.KEYEVENTF_KEYUP, UIntPtr.Zero);
            //WindowsKeyboard.keybd_event(WindowsKeyboard.VK_CONTROL, 0, WindowsKeyboard.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }



        private const int CHILDID_SELF = 0;
        private const uint OBJID_WINDOW = 0x00000000;
        private const uint OBJID_SYSMENU = 0xFFFFFFFF;
        private const uint OBJID_TITLEBAR = 0xFFFFFFFE;
        private const uint OBJID_MENU = 0xFFFFFFFD;
        private const uint OBJID_CLIENT = 0xFFFFFFFC;
        private const uint OBJID_VSCROLL = 0xFFFFFFFB;
        private const uint OBJID_HSCROLL = 0xFFFFFFFA;
        private const uint OBJID_SIZEGRIP = 0xFFFFFFF9;
        private const uint OBJID_CARET = 0xFFFFFFF8;
        private const uint OBJID_CURSOR = 0xFFFFFFF7;
        private const uint OBJID_ALERT = 0xFFFFFFF6;
        private const uint OBJID_SOUND = 0xFFFFFFF5;

        [DllImport("oleacc.dll")]
        private static extern int AccessibleObjectFromWindow(
            IntPtr hwnd,
            uint id,
            ref Guid iid,
            [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object ppvObject);

        [DllImport("user32.dll")]
        private static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [Flags]
        private enum GuiThreadInfoFlags
        {
            GUI_CARETBLINKING = 0x00000001,
            GUI_INMENUMODE = 0x00000004,
            GUI_INMOVESIZE = 0x00000002,
            GUI_POPUPMENUMODE = 0x00000010,
            GUI_SYSTEMMENUMODE = 0x00000008
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct GUITHREADINFO
        {
            public int cbSize;
            public GuiThreadInfoFlags flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public System.Drawing.Rectangle rcCaret;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetCaretPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private static string GetText(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}
