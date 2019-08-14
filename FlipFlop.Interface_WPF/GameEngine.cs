using FlipFlop.Interface_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipFlop.Interface_WPF
{
    class GameEngine
    {
        readonly Deck Deck = new Deck();
        readonly MainWindow MainWindow;
        readonly List<Player> Players;
        readonly Board Board;

        public Player ActivePlayer;
        PlayerCard SelectedCard;
        
        public GameEngine (MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            Players = new List<Player> { new Player(Deck, MainWindow, 1), new Player(Deck, MainWindow, 2) };
            Board = new Board(Deck, MainWindow);
        }

        internal void SetupFirstGame()
        {
            Players.ForEach(x => x.DrawNewHand());
            MainWindow.UpdateDeckSize(Deck.CardList.Count);
            ActivePlayer = Players[0];
        }

        internal void SwitchActivePlayer()
        {
            ActivePlayer = Players[ActivePlayer.Id == 1 ? 1 : 0];
        }

        private void RoundOver()
        {
            if (Board.Full)
                GameEnd();
            else
                NewRound();
        }

        private void NewRound()
        {
            HideHand();
            DeselectPlayerCard();
            SwitchActivePlayer();
            MainWindow.ShowNextRoundPopup();
        }

        internal void StartRound()
        {
            Board.ResetSpaceColors();
            ShowHand();
        }
        internal void ShowHand()
        {
            ActivePlayer.ShowHand();
        }
        internal void HideHand()
        {
            ActivePlayer.HideHand();
        }
        public void SelectPlayerCard(string clickedCardName)
        {
            if (SelectedCard != null)
                DeselectPlayerCard();

            PlayerCard clickedCard = ActivePlayer.Hand.Single(x => x.Name == clickedCardName);
            if (clickedCard.CanBeSelected())
            {
                SelectedCard = clickedCard;
                SelectedCard.SetColorSelected();
            }

        }

        public void DeselectPlayerCard()
        {
            if (SelectedCard != null)
            {
                SelectedCard.ResetColor();
                SelectedCard = null;
            }
        }

        internal void PlayCard(string boardSpaceName)
        {
            if (SelectedCard == null || SelectedCard.IsEmpty())
                return;

            BoardSpace space = Board.GetByName(boardSpaceName);

            if (!space.IsEmpty())
                return;

            Card playedCard = SelectedCard.TakeCard();
            space.PlaceCard(playedCard);
            Board.FlipFlopCard(space, ActivePlayer.Id);

            RoundOver();
        }

        private void GameEnd()
        {
            int score = Board.AddPlayerScore(Players);
            ActivePlayer = Players[Board.Winner()];
            MainWindow.UpdateScoreBox();

            if (Deck.EnoughCardsLeft())
            {
                MainWindow.ShowNewGamePopup(score);
            }
            else
            {
                ActivePlayer = Players.OrderByDescending(x => x.Score).First();
                MainWindow.ShowMatchEndPopup();
            }

        }

        public void SetupNewGame()
        {
            Board.Discard();
            Board.Clear();
            Players.ForEach(x => x.DrawNewHand());
            MainWindow.UpdateDeckSize(Deck.CardList.Count);
            NewRound();
        }

    }
}
