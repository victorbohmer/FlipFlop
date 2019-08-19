using FlipFlop.Interface_WPF.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.AI
{
    public class Aziraphale : AIPlayer
    {
        public Aziraphale(Board board, Player player)
        {
            Board = board;
            Player = player;
            Name = "Aziraphale (AI)";
        }

        public override PlayerCard SelectCardToPlay()
        {
            List<PlayerCard> orderedHand = Player.Hand.Where(x => x.Card != null).OrderByDescending(x => x.Card.Value).ToList();

            //Cards are randomly selected from a set spread depending on how many cards have already been played
            //Example: If playing first the AI will pick a random card most turns, but always play its lowest card second and
            //its highest card last
            switch (Board.PlayedCardCount)
            {
                //AI plays first
                case 0:
                    return orderedHand[random.Next(1, 4)];
                case 2:
                    return orderedHand[3];
                case 4:
                    return orderedHand[random.Next(1, 3)];
                case 6:
                    return orderedHand[1];
                case 8:
                    return orderedHand[0];
                //AI plays second
                case 1:
                    return orderedHand[random.Next(2, 5)];
                case 3:
                    return orderedHand[random.Next(1, 4)];
                case 5:
                    return orderedHand[random.Next(0, 2)];
                case 7:
                    return orderedHand[0];
                default:
                    throw new ArgumentException("Board state invalid");

            }
        }

        public override BoardCard SelectSpaceToPlayOn(PlayerCard selectedCard)
        {
            //The AI will always take the highest card it can on the board where possible
            List<BoardCard> spacesThatCanBeTaken = Board.Spaces.Where(x =>
                x.Card != null &&
                x.Card.Value < selectedCard.Card.Value &&
                x.Owner != 2)
                .OrderByDescending(x => x.Card.Value)
                .ToList();

            if (spacesThatCanBeTaken != null)
            {
                foreach (BoardCard boardSpace in spacesThatCanBeTaken)
                {
                    BoardCard emptyNeighbouringSpace = FindRandomEmptyNeighbouringSpace(boardSpace);
                    if (emptyNeighbouringSpace != null)
                        return emptyNeighbouringSpace;
                }
            }

            return Board.RandomEmptySpace();

        }

        private BoardCard FindRandomEmptyNeighbouringSpace(BoardCard boardSpace)
        {
            List<Direction> directions = new List<Direction> { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

            for (int i = 0; i < 4; i++)
            {
                Direction randomDirection = directions[random.Next(directions.Count)];
                directions.Remove(randomDirection);
                BoardCard neighbouringSpace = Board.GetNeighbour(boardSpace, randomDirection);
                if (neighbouringSpace != null && neighbouringSpace.Card == null)
                {
                    return neighbouringSpace;
                }
            }
            //returns null if it fails to find an empty neighbour
            return null;
        }

    }
}
