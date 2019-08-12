using System;
using System.Collections.Generic;
using System.Text;

namespace FlipFlop.Game
{
    public class Card
    {
        public int Value { get; }
        public Suit Suit { get; }
        public Card(int i, int suit)
        {
            Value = i;
            Suit = (Suit)suit;
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }

    }
}
