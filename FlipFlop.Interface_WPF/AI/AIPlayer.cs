using FlipFlop.Interface_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.AI
{
    abstract public class AIPlayer
    {
        protected Board Board;
        public Player Player;
        public string Name { get; protected set; }
        protected readonly Random random = new Random();

        abstract public PlayerCard SelectCardToPlay();

        abstract public BoardSpace SelectSpaceToPlayOn(PlayerCard selectedCard);

    }
}
