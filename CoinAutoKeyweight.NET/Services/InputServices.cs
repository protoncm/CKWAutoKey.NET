using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoinAutoKeyweight.NET.Services
{
    public class InputServices
    {
        public static void PressKey(string keyString, bool press)
        {
            KeyConverter k = new KeyConverter();
            Key key = (Key)k.ConvertFromString(keyString);

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            ushort scanCode = (ushort)WindowsAPI.MapVirtualKey((uint)virtualKey, 0);

            if (press)
                KeyDown(scanCode);
            else
                KeyUp(scanCode);
        }

        public static void ReleaseKey(string keyString)
        {
            KeyConverter k = new KeyConverter();
            Key key = (Key)k.ConvertFromString(keyString);
            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            ushort scanCode = (ushort)WindowsAPI.MapVirtualKey((uint)virtualKey, 0);
            WindowsAPI.keybd_event((byte)virtualKey, (byte)scanCode, WindowsAPI.KEYEVENTF_KEYUP, 0);
        }

        public static void KeyDown(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_SCANCODE;
            inputs[0].ki.wScan = (ushort)(scanCode & 0xff);
            inputs[0].ki.time = 0;
            uint intReturn = WindowsAPI.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

        public static void KeyUp(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.wScan = scanCode;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP;
            inputs[0].ki.time = 0;
            uint intReturn = WindowsAPI.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }
    }
}
