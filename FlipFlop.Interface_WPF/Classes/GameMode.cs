using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.GameClasses
{
    static class GameMode
    {
        static public bool AI { get; private set; } = true;
        static public int GameEndDeckSize { get; private set; } = 30;
        public static string GameLength { get { return GameEndDeckSize == 30 ? "Short" : "Long"; } }

        public static void AIModeOn()
        {
            AI = true;
        }

        public static void AIModeOff()
        {
            AI = false;
        }

        public static void GameLengthShort()
        {
            GameEndDeckSize = 30;
        }

        public static void GameLengthLong()
        {
            GameEndDeckSize = 10;
        }
    }
}
