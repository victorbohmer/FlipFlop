using FlipFlop.Interface_WPF.AI;
using FlipFlop.Interface_WPF.GameClasses;
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
        readonly AIPlayer AIPlayer;
        public Player ActivePlayer { get; private set; }
        public string ActivePlayerName
        {
            get
            {
                return GameMode.AI && ActivePlayer.Id == 2 ? AIPlayer.Name : ActivePlayer.Name;
            }
        }
        public void SwitchActivePlayer()
        {
            ActivePlayer = Players[ActivePlayer.Id == 1 ? 1 : 0];
        }
        public PlayerCard SelectedCard { get; private set; }


        public GameEngine(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            Players = new List<Player> { new Player(Deck, MainWindow, 1), new Player(Deck, MainWindow, 2) };
            Board = new Board(Deck, MainWindow);
            AIPlayer = new Aziraphale(Board, Players[1]);
        }


        public void SetupFirstGame()
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
            NewRound(true);
        }

        
        private void RoundOver()
        {
            if (Board.Full)
                GameEnd();
            else
                NewRound();
        }
        private void NewRound(bool firstRound = false)
        {
            HideHand();
            DeselectPlayerCard();
            SwitchActivePlayer();

            if (ActivePlayer.Id == 1)
            {
                if (GameMode.AI && !firstRound)
                    StartRound();
                else
                    MainWindow.ShowNextRoundPopup();
            }
            else
            {
                if (GameMode.AI)
                    PlayAITurn();
                else
                    MainWindow.ShowNextRoundPopup();
            }
        }
        private void PlayAITurn()
        {
            Board.ResetSpaceColors();
            SelectedCard = AIPlayer.SelectCardToPlay();
            BoardCard boardSpace = AIPlayer.SelectSpaceToPlayOn(SelectedCard);

            PlayCard(boardSpace);
        }
        public void StartRound()
        {
            ShowHand();
        }


        public void SelectPlayerCard(string clickedCardName)
        {
            DeselectPlayerCard();
            PlayerCard clickedCard;

            try
            {
                clickedCard = ActivePlayer.Hand.Single(x => x.Name == clickedCardName);
                if (clickedCard.CanBeSelected())
                {
                    SelectedCard = clickedCard;
                    SelectedCard.Selected();
                    Board.ResetSpaceColors();
                }
            }
            catch (Exception)
            {
                return;
            }

            
        }
        public void DeselectPlayerCard()
        {
            if (SelectedCard != null)
            {
                SelectedCard.Unselected();
                SelectedCard = null;
            }
        }


        public void TryToPlayCard(string boardSpaceName)
        {
            if (SelectedCard == null || SelectedCard.IsEmpty)
                return;

            BoardCard boardSpace = Board.GetByName(boardSpaceName);

            if (!boardSpace.IsEmpty)
                return;

            PlayCard(boardSpace);
        }
        private void PlayCard(BoardCard boardSpace)
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
        public void CleanUpBeforeNewMatch()
        {
            Board.Clear();
            Board.ResetSpaceColors();
        }


        public void ShowHand()
        {
            ActivePlayer.ShowHand();
        }
        public void HideHand()
        {
            ActivePlayer.HideHand();
        }
        public void UpdatePlayerNames(string player1Name, string player2Name)
        {
            Players[0].Name = player1Name;
            Players[1].Name = player2Name;

            MainWindow.UpdateNameBox(1, player1Name);

            if (GameMode.AI)
                MainWindow.UpdateNameBox(2, AIPlayer.Name);
            else
                MainWindow.UpdateNameBox(2, player2Name);

        }


        public string GetFullRecordText()
        {
            List<MatchRecord> matchRecords = FileHandler.GetRecordsFromFile();
            return String.Join("\n", matchRecords.Select(x => x.ToString()));
        }
    }
}
