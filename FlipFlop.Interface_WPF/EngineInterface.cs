using FlipFlop.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlipFlop.Interface_WPF
{
    class EngineInterface
    {
        Engine Engine = new Engine();
        MainWindow MainWindow;
        Card SelectedCard;
        ImageSource NoCard;
        internal void SetupGame(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            NoCard = (ImageSource)MainWindow.FindResource("card back");
            Engine.DrawNewHands();
            ShowHands();
        }

        private void ShowHands()
        {
            for (int playerIndex = 1; playerIndex <= 2; playerIndex++)
            {
                List<string> playercards = Engine.GetCardNames(playerIndex);

                for (int cardIndex = 1; cardIndex <= playercards.Count; cardIndex++)
                {
                    string imageObjectName = $"Player_{playerIndex}_Card_{cardIndex}_Image";
                    Image imageObject = (Image)MainWindow.FindName(imageObjectName);
                    imageObject.Source = (ImageSource)MainWindow.FindResource(playercards[cardIndex - 1]);
                } 
            }
        }

        internal void PlayCard(Button clickedSpace, ref Button selectedButton)
        {
            ImageSource playedCardImage = MainWindow.GetImage(selectedButton).Source;

            if (!NoCardOnSpace(selectedButton) && NoCardOnSpace(clickedSpace))
            {
                SetCardOnSpace(clickedSpace, playedCardImage);
                RemoveCardFromHand(selectedButton);
                selectedButton.Background = new SolidColorBrush(Colors.AliceBlue);
                selectedButton = null;
            }
        }

        private void SetCardOnSpace(Button button, ImageSource playedCardImage)
        {
            MainWindow.GetImage(button).Source = playedCardImage;
        }

        private void RemoveCardFromHand(Button button)
        {
            MainWindow.GetImage(button).Source = null;
        }

        private bool NoCardOnSpace(Button button)
        {
            return MainWindow.GetImage(button).Source == null;
        }

    }
}
