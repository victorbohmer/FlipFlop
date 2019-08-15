using FlipFlop.Interface_WPF.AI;
using FlipFlop.Interface_WPF.Classes;
using FlipFlop.Interface_WPF.RecordKeeping;
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
        readonly FileHandler FileHandler = new FileHandler();
        public Player ActivePlayer { get; private set; }
        PlayerCard SelectedCard;
        readonly AIPlayer AIPlayer;

        public GameEngine(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            Players = new List<Player> { new Player(Deck, MainWindow, 1), new Player(Deck, MainWindow, 2) };
            Board = new Board(Deck, MainWindow);
            AIPlayer = new Aziraphale(Board, Players[1]);
        }

        internal void SetupFirstGame()
        {
            ActivePlayer = Players[0];
            MainWindow.UpdateScoreBox();
            SwitchActivePlayer();
            MainWindow.UpdateScoreBox();
            TryToSetupNewGame();
        }
        public void TryToSetupNewGame()
        {
            Board.Discard();
            Board.Clear();
            Board.ResetSpaceColors();

            if (Deck.EnoughCardsLeft())
                SetupNewGame();
            else
                EndMatch();
        }

        private void SetupNewGame()
        {
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

            MainWindow.ShowNewGamePopup(score);

        }

        private void EndMatch()
        {
            List<Player> sortedPlayers = Players.OrderByDescending(x => x.Score).ToList();

            MatchRecord record = new MatchRecord(sortedPlayers, AIPlayer.Name);
            FileHandler.SaveMatchRecord(record);

            ActivePlayer = sortedPlayers.First();
            MainWindow.ShowMatchEndPopup();
        }

        internal void CleanUpBeforeNewMatch()
        {
            Board.Clear();
            Board.ResetSpaceColors();
        }

        internal void UpdatePlayerNames(string player1Name, string player2Name)
        {
            Players[0].Name = player1Name;
            Players[1].Name = player2Name;

            MainWindow.UpdateNameBox(1, player1Name);

            if (GameMode.AI)
                MainWindow.UpdateNameBox(2, AIPlayer.Name);
            else
                MainWindow.UpdateNameBox(2, player2Name);

        }

        internal string GetFullRecordText()
        {
            List<MatchRecord> matchRecords = FileHandler.GetRecordsFromFile();
            return String.Join("\n", matchRecords.Select(x => x.ToString()));
        }
    }
}
