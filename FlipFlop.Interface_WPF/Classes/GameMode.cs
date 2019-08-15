using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.Classes
{
    static class GameMode
    {
        static public bool AI { get; private set; } = true;
        static public int GameEndDeckSize { get; private set; } = 30;
        public static string GameLength { get { return GameEndDeckSize == 30 ? "Short" : "Long"; } }

        internal static void AIModeOn()
        {
            AI = true;
        }

        internal static void AIModeOff()
        {
            AI = false;
        }

        internal static void GameLengthShort()
        {
            GameEndDeckSize = 30;
        }

        internal static void GameLengthLong()
        {
            GameEndDeckSize = 10;
        }
    }
}
