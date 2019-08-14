using FlipFlop.Interface_WPF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlipFlop.Interface_WPF.Classes
{
    public class BoardSpace : CardSpace
    {
        public int Index { get; set; }
        public int Owner { get; set; }
        public BoardSpace(int spaceIndex, MainWindow mainWindow)
        {
            MainWindow = mainWindow;

            Index = spaceIndex;

            string buttonName = $"Played_Card_{spaceIndex}";
            string imageName = buttonName + "_Image";

            WPFButton = (Button)MainWindow.FindName(buttonName);
            WPFImage = (Image)MainWindow.FindName(imageName);
        }

        public void ReturnCard(Deck deck)
        {
            if (!IsEmpty())
            {
                Owner = 0;
                deck.ReturnCard(TakeCard());
            }
        }

        internal void ChangeOwner(int playerId)
        {
            Owner = playerId;
            WPFButton.RenderTransform = new RotateTransform(playerId == 1 ? 0 : 90);
        }

        internal void ResetColor()
        {
            MainWindow.SetBackgroundColor(WPFButton, WPFColor.Grid);
        }

        internal void SetColorFlipped()
        {
            MainWindow.SetBackgroundColor(WPFButton, WPFColor.Popup);
        }
    }
}
