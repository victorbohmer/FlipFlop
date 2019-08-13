using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlipFlop.Interface_WPF.Classes
{
    public class PlayerCard : CardSpace
    {
        
        public PlayerCard(int playerId, int cardIndex, MainWindow mainWindow)
        {
            string buttonName = $"Player_{playerId}_Card_{cardIndex}";
            string imageName = buttonName + "_Image";

            MainWindow = mainWindow;

            WPFButton = (Button)MainWindow.FindName(buttonName);
            WPFImage = (Image)MainWindow.FindName(imageName);
            NoCardImage = (ImageSource)MainWindow.FindResource("card back");
        }

        public void DrawNew(Deck deck)
        {
            if (!IsEmpty())
                deck.ReturnCard(TakeCard());

            Card = deck.Draw();
        }

        public void ShowCard()
        {
            UpdateImage();
        }
        public void HideCard()
        {
            WPFImage.Source = NoCardImage;
        }

        internal bool CanBeSelected()
        {
            return WPFImage.Source != NoCardImage;
        }
    }
}
