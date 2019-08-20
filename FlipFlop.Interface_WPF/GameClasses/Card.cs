using System;
using System.Collections.Generic;
using System.Text;

namespace FlipFlop.Interface_WPF.GameClasses
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
