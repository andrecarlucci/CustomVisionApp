using Dear;
using Dear.KeyboardControl;
using System.Diagnostics;
using System.Threading;

namespace CustomVisionApp.StreetFighter {
    public static class SpecialAtacks {

        public static void Execute(string attack) {
            if (attack == "hadouken") {
                Hadouken();
                Thread.Sleep(1000);
            }
            else if (attack == "shoryuken") {
                Shoryuken();
                Thread.Sleep(1000);
            }
            else {
                Debug.WriteLine("->>>>>>>>> " + attack);
            }
        }

        public static void Hadouken() {
            Debug.WriteLine("->>>>>>>>> hadouken");
            var k = new MrWindows().Keyboard;
            k.Press(VirtualKey.Down).Wait(100)
             .Press(VirtualKey.Right).Wait(20)
             .Release(VirtualKey.Down)
             .Press(VirtualKey.D).Wait(100)
             .Release(VirtualKey.Right)
             .Release(VirtualKey.D);
        }

        public static void Shoryuken() {
            Debug.WriteLine("->>>>>>>>>  shoryuken");
            var k = new MrWindows().Keyboard;

            k.Press(VirtualKey.Right).Wait(100);
            k.Release(VirtualKey.Right);
            k.Press(VirtualKey.Down).Wait(100);
            k.Press(VirtualKey.Right).Wait(100);
            k.Press(VirtualKey.D).Wait(100);
            k.Release(VirtualKey.Down);
            k.Release(VirtualKey.Right);
            k.Release(VirtualKey.D);
        }
    }
}
