using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest.KeyboardHook
{
    internal static class GlobalKeyboardHookImpl
    {
        public static void Start()
        {
            var listener = new LowLevelKeyboardListener();
            listener.OnKeyPressed += Listener_OnKeyPressed;
            listener.HookKeyboard();

            Thread.Sleep(10000);

            listener.UnHookKeyboard();
        }

        private static void Listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            Console.WriteLine($"Pressed: {e/*.KeyPressed*/}");
        }
    }
}
