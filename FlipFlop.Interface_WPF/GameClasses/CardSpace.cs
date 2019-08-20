using FlipFlop.Interface_WPF.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.Classes
{
    public abstract class CardSpace
    {
        public Card Card { get; protected set; }
        public int Index { get; set; }
        public bool IsEmpty { get { return Card == null; } }
        public abstract void PlaceCard(Card card);
        public abstract Card TakeCard();
        public abstract void ReturnCard(Deck deck);
    }
}
