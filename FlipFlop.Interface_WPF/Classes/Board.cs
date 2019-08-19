using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlipFlop.Interface_WPF.Classes
{
    public class Board
    {
        readonly Random random = new Random();
        readonly Deck Deck;
        public List<BoardSpace> Spaces { get; private set; } = new List<BoardSpace>();
        public bool Full { get { return PlayedCardCount == 9; } }
        public int PlayedCardCount { get { return Spaces.Where(x => x.Card != null).Count(); } }
        public Board(Deck deck, MainWindow mainWindow)
        {
            Deck = deck;
            CreateSpaces(mainWindow);
        }
        public Board(Deck deck)
        {
            Deck = deck;
            CreateSpaces();
        }

        private void CreateSpaces(MainWindow mainWindow)
        {
            for (int spaceIndex = 1; spaceIndex <= 9; spaceIndex++)
            {
                Spaces.Add(new BoardSpace(spaceIndex, mainWindow));
            }
        }
        private void CreateSpaces()
        {
            for (int spaceIndex = 1; spaceIndex <= 9; spaceIndex++)
            {
                Spaces.Add(new BoardSpace(spaceIndex));
            }
        }
        public void Clear()
        {
            Spaces.ForEach(x => x.ReturnCard(Deck));
        }

        public void ResetSpaceColors()
        {
            Spaces.ForEach(x => x.ResetColor());
        }

        public void FlipFlopCard(BoardSpace clickedSpace, int activePlayerId)
        {
            clickedSpace.ChangeOwner(activePlayerId);
            clickedSpace.SetColorFlipped();
            TryToFlipFlopNeighbours(clickedSpace);
        }

        private void TryToFlipFlopNeighbours(BoardSpace flippedSpace)
        {

            foreach (Direction direction in new[] { Direction.Right, Direction.Left, Direction.Up, Direction.Down })
            {
                BoardSpace neighbouringSpace = GetNeighbour(flippedSpace, direction);

                if (neighbouringSpace != null)
                    TryToFlipFlopSpace(flippedSpace, neighbouringSpace);
            }

        }

        private void TryToFlipFlopSpace(BoardSpace flippedSpace, BoardSpace neighbouringSpace)
        {
            if (neighbouringSpace.Card != null && 
                neighbouringSpace.Owner != flippedSpace.Owner &&
                neighbouringSpace.Card.Value < flippedSpace.Card.Value)
            {
                FlipFlopCard(neighbouringSpace, flippedSpace.Owner);
            }

        }

        public BoardSpace RandomEmptySpace()
        {
            List<BoardSpace> emptySpaces = Spaces.Where(x => x.Card == null).ToList();
            return emptySpaces[random.Next(emptySpaces.Count)];
        }

        public BoardSpace GetByName(string boardSpaceName)
        {
            return Spaces.Single(x => x.Name == boardSpaceName);
        }

        public BoardSpace GetNeighbour(BoardSpace clickedSpace, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (clickedSpace.Index % 3 == 1)
                        return null;
                    else
                        return Spaces.Single(x => x.Index == clickedSpace.Index - 1);
                case Direction.Right:
                    if (clickedSpace.Index % 3 == 0)
                        return null;
                    else
                        return Spaces.Single(x => x.Index == clickedSpace.Index + 1);
                case Direction.Up:
                    if (clickedSpace.Index > 6)
                        return null;
                    else
                        return Spaces.Single(x => x.Index == clickedSpace.Index + 3);
                case Direction.Down:
                    if (clickedSpace.Index < 4)
                        return null;
                    else
                        return Spaces.Single(x => x.Index == clickedSpace.Index - 3);
                default:
                    break;
            }
            return null;
        }

        public void Discard()
        {
            int score = CalculateScore();
            for (int i = 0; i < score; i++)
            {
                List<BoardSpace> filledSpaces = Spaces.Where(x => x.Card != null).ToList();
                filledSpaces[random.Next(filledSpaces.Count)].TakeCard();
            }
        }

        public int Winner()
        {
            return Spaces.Where(x => x.Owner == 1).Count() > 4 ? 0 : 1;
        }

        public int WinnersControlledSpaces()
        {
            return Spaces.Where(x => x.Owner == Winner() + 1).Count();
        }
        public int AddPlayerScore(List<Player> players)
        {
            int score = CalculateScore(); 
            players[Winner()].Score += score;
            return score;
        }

        private int CalculateScore()
        {
            return WinnersControlledSpaces() * 2 - 9;
        }
    }
}
