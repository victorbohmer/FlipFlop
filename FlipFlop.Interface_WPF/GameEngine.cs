using FlipFlop.Interface_WPF.AI;
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
        AIPlayer AIPlayer;

        
        public GameEngine (MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            Players = new List<Player> { new Player(Deck, MainWindow, 1), new Player(Deck, MainWindow, 2) };
            Board = new Board(Deck, MainWindow);
            AIPlayer = new Aziraphale(Board, Players[1]);
        }

        internal void SetupFirstGame()
        {
            Board.Clear();
            ActivePlayer = Players[1];
            SetupNewGame();
        }
        public void SetupNewGame()
        {
            Board.Discard();
            Board.Clear();
            Board.ResetSpaceColors();
            Players.ForEach(x => x.DrawNewHand());
            MainWindow.UpdateDeckSize(Deck.CardList.Count);
            NewRound();
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

            if (IsAIsTurn())
            {
                PlayAITurn();
            }
            else
                MainWindow.ShowNextRoundPopup();

        }

        private bool IsAIsTurn()
        {
            return GameMode.AI && ActivePlayer.Id == 2;
        }

        private void PlayAITurn()
        {
            Board.ResetSpaceColors();
            SelectedCard = AIPlayer.SelectCardToPlay();
            BoardSpace boardSpace = AIPlayer.SelectSpaceToPlayOn(SelectedCard);

            PlayCard(boardSpace);
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

        internal void TryToPlayCard(string boardSpaceName)
        {
            if (SelectedCard == null || SelectedCard.IsEmpty())
                return;

            BoardSpace boardSpace = Board.GetByName(boardSpaceName);

            if (!boardSpace.IsEmpty())
                return;

            PlayCard(boardSpace);

        }

        private void PlayCard(BoardSpace boardSpace)
        {
            Card playedCard = SelectedCard.TakeCard();
            boardSpace.PlaceCard(playedCard);
            Board.FlipFlopCard(boardSpace, ActivePlayer.Id);

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

    }
}
