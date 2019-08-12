using FlipFlop.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlipFlop.Interface_WPF
{
    class EngineInterface
    {
        Engine Engine = new Engine();
        MainWindow MainWindow;
        ImageSource cardBack;
        public int ActivePlayer { get; private set; }

        internal void SetupFirstGame(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            cardBack = (ImageSource)MainWindow.FindResource("card back");
            

            Engine.DrawNewHands();
            ActivePlayer = 1;
        }

        public void ShowHand()
        {
            List<string> playercards = Engine.GetCardNames(ActivePlayer);

            for (int cardIndex = 1; cardIndex <= playercards.Count; cardIndex++)
            {
                string imageObjectName = $"Player_{ActivePlayer}_Card_{cardIndex}_Image";
                Image imageObject = (Image)MainWindow.FindName(imageObjectName);
                imageObject.Source = (ImageSource)MainWindow.FindResource(playercards[cardIndex - 1]);
            }

        }

        internal void PlayCard(Button clickedSpace, Button selectedCard)
        {
            ImageSource playedCardImage = MainWindow.GetImage(selectedCard).Source;


            if (!NoCardOnSpace(selectedCard) && NoCardOnSpace(clickedSpace))
            {
                SetCardOnSpace(clickedSpace, playedCardImage);
                ActivePlayer = SetActivePlayer(selectedCard);
                Engine.PlayCard(GetCardName(selectedCard), ActivePlayer);
                FlipFlopCard(clickedSpace);

                MainWindow.DeselectCard();
                
                RoundOver();
            }
        }

        private void RoundOver()
        {
            if (AllSpacesFilled())
                GameEnd();
            else
                NewRound();
        }

        private void NewRound()
        {
            HideHand();

            Button nextRoundPopupText = (Button)MainWindow.FindName("NextRoundPopupText");
            ActivePlayer = ActivePlayer == 1 ? 2 : 1;
            nextRoundPopupText.Content = $"Player {ActivePlayer}'s turn!";

            Popup nextRoundPopup = (Popup)MainWindow.FindName("NextRoundPopup");
            nextRoundPopup.VerticalOffset = ActivePlayer == 1 ? -268 : 268;
            nextRoundPopup.IsOpen = true;
        }

        private void GameEnd()
        {
            int score = CountSpaces();

            if (score > 0)
                ActivePlayer = 1;
            else
            {
                score = -score;
                ActivePlayer = 2;
            }

            Engine.RemovePlayedCards(score);
            Engine.ShuffleBackIntoDeck();
            AddScore(score);
            if (Engine.EnoughCardsLeft())
                ShowEndGamePopup(score);
            else
                MatchEnd();
            
        }

        private void ClearBoard()
        {
            for (int xPosition = 1; xPosition <= 3; xPosition++)
            {
                for (int yPosition = 1; yPosition <= 3; yPosition++)
                {
                    Button space = (Button)MainWindow.FindName($"Played_Card_{yPosition}_{xPosition}");
                    MainWindow.GetImage(space).Source = null;
                }

            }
        }

        private void MatchEnd()
        {
            TextBlock playerScoreBox = (TextBlock)MainWindow.FindName($"Player_{ActivePlayer}_Score");
            string playerPoints = playerScoreBox.Text;

            TextBlock matchEndPopupText = (TextBlock)MainWindow.FindName("MatchEndPopupText");
            matchEndPopupText.Text = $"Player {ActivePlayer} won with {playerPoints} points!";

            Popup matchEndPopup = (Popup)MainWindow.FindName("MatchEndPopup");
            matchEndPopup.IsOpen = true;
        }

        private void ShowEndGamePopup(int score)
        {
            TextBlock newGamePopupText = (TextBlock)MainWindow.FindName("NewGamePopupText");
            newGamePopupText.Text = $"Player {ActivePlayer} controlled {5 + (score - 1) / 2} spaces and got {score} points! There are {Engine.CardsLeft()} cards left to play for.";

            Popup newGamePopup = (Popup)MainWindow.FindName("NewGamePopup");
            newGamePopup.IsOpen = true;
            newGamePopup.VerticalOffset = ActivePlayer == 1 ? -268 : 268;
        }

        public void SetupNewGame()
        {
            ClearBoard();
            Engine.DrawNewHands();
            NewRound();
        }

        private void AddScore(int score)
        {
            TextBlock playerScoreBox = (TextBlock)MainWindow.FindName($"Player_{ActivePlayer}_Score");
            playerScoreBox.Text = (int.Parse(playerScoreBox.Text) + score).ToString();
        }

        private int CountSpaces()
        {
            int score = 0;
            ActivePlayer = 1;
            for (int xPosition = 1; xPosition <= 3; xPosition++)
            {
                for (int yPosition = 1; yPosition <= 3; yPosition++)
                {
                    Button space = (Button)MainWindow.FindName($"Played_Card_{yPosition}_{xPosition}");
                    if (CardBelongsToOtherPlayer(space))
                        score--;
                    else
                        score++;

                    //MainWindow.GetImage(space).Source = null;
                }

            }
            return score;
        }

        private bool AllSpacesFilled()
        {
            for (int xPosition = 1; xPosition <= 3; xPosition++)
            {
                for (int yPosition = 1; yPosition <= 3; yPosition++)
                {
                    Button space = (Button)MainWindow.FindName($"Played_Card_{yPosition}_{xPosition}");
                    if (NoCardOnSpace(space))
                        return false;
                }
            }
            return true;
        }

        private void HideHand()
        {
            for (int cardIndex = 1; cardIndex <= 5; cardIndex++)
            {
                string imageObjectName = $"Player_{ActivePlayer}_Card_{cardIndex}_Image";
                Image imageObject = (Image)MainWindow.FindName(imageObjectName);
                imageObject.Source = cardBack;
            }
        }

        private int SetActivePlayer(Button selectedCard)
        {
            return int.Parse(selectedCard.Name.Split('_')[1]);
        }

        private void FlipFlopCard(Button clickedSpace)
        {
            RotateCard(clickedSpace);

            TryToFlipFlopNeighbours(clickedSpace);

        }

        private void RotateCard(Button clickedSpace)
        {
            clickedSpace.RenderTransform = new RotateTransform(ActivePlayer == 1 ? 0 : 90);
        }

        private void TryToFlipFlopNeighbours(Button clickedSpace)
        {
            int yPosition = int.Parse(clickedSpace.Name.Split('_')[2]);
            int xPosition = int.Parse(clickedSpace.Name.Split('_')[3]);

            int cardValue = GetCardValue(clickedSpace);

            if (yPosition > 1)
            {
                TryToFlipFlopPosition(yPosition - 1, xPosition, cardValue);
            }
            if (yPosition < 3)
            {
                TryToFlipFlopPosition(yPosition + 1, xPosition, cardValue);
            }
            if (xPosition > 1)
            {
                TryToFlipFlopPosition(yPosition, xPosition - 1, cardValue);
            }
            if (xPosition < 3)
            {
                TryToFlipFlopPosition(yPosition, xPosition + 1, cardValue);
            }

        }

        private void TryToFlipFlopPosition(int yPosition, int xPosition, int cardValue)
        {
            string targetSpaceName = $"Played_Card_{yPosition}_{xPosition}";
            Button targetSpace = (Button)MainWindow.FindName(targetSpaceName);

            if (!NoCardOnSpace(targetSpace) &&
                CardBelongsToOtherPlayer(targetSpace) &&
                CardHasLowerValue(targetSpace, cardValue))
            {
                FlipFlopCard(targetSpace);
            }

        }

        private bool CardHasLowerValue(Button targetSpace, int cardValue)
        {
            return GetCardValue(targetSpace) < cardValue;
        }

        private bool CardBelongsToOtherPlayer(Button targetSpace)
        {
            var rotation = (RotateTransform)targetSpace.RenderTransform;
            return rotation.Angle != (ActivePlayer == 1 ? 0 : 90);
        }

        private int GetCardValue(Button clickedSpace)
        {
            BitmapImage imageSource = (BitmapImage)MainWindow.GetImage(clickedSpace).Source;
            string uriString = imageSource.UriSource.OriginalString;
            return int.Parse(uriString.Replace("/", " ").Split(' ')[1]);
        }
        private string GetCardName(Button clickedSpace)
        {
            BitmapImage imageSource = (BitmapImage)MainWindow.GetImage(clickedSpace).Source;
            string uriString = imageSource.UriSource.OriginalString;
            return uriString.Split('/')[1].Split('.')[0];
        }

        private void SetCardOnSpace(Button button, ImageSource playedCardImage)
        {
            MainWindow.GetImage(button).Source = playedCardImage;
        }

        private bool NoCardOnSpace(Button button)
        {
            if (MainWindow.GetImage(button).Source == null)
                return true;
            else if (MainWindow.GetImage(button).Source == cardBack)
                return true;
            else
                return false;
        }

    }
}
