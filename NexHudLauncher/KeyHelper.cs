using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexHudLauncher
{
    public class KeyHelper
    {
        public static Key[] modifiers = new Key[] {

            Key.LControl,
            Key.RControl,
            Key.LAlt,
            Key.RAlt,
            Key.LShift,
            Key.RShift,
        };
        public static bool isModifier(Key k)
        {
            return modifiers.Contains(k);
        }

        public static string keyToString(Key k)
        {
            switch(k)
            {
                 case Key.LControl: return "Left Ctrl";
                case Key.RControl: return "Right Ctrl";
                case Key.LAlt: return "Left Alt";
                case Key.RAlt: return "Right Alt";
                case Key.LShift: return "Left Shift";
                case Key.RShift: return "Right Shift";
            }
            return k.ToString();
        }
    }
}
