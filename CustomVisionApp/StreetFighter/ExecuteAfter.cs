using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomVisionApp.StreetFighter {
    public static class ExecuteWhen {

        private static string _current = "";
        private static int _count = 0;
        private static Dictionary<string, Action> _dic = new Dictionary<string, Action>();

        public static void Register(string value, Action action) {
            _dic[value] = action;
        }

        public static void SameValueThreeTimes(string value) {
            if(!_dic.ContainsKey(value) || _current != value) {
                _count = 0;
                _current = value;
                return;
            }
            _count++;
            if(_count == 3) {
                _dic[value].Invoke();
                _count = 0;
                Thread.Sleep(1000);
            }
        }
    }
}
