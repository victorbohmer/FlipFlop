using FlipFlop.Interface_WPF.Classes;
using FlipFlop.Interface_WPF.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.Classes_WPF
{
    public abstract class CardSpaceWPF : CardSpace
    {
        protected WPFCardObject CardObject;
        public override void PlaceCard(Card card)
        {
            Card = card;
            UpdateInterfaceObject();
        }
        public override Card TakeCard()
        {
            Card returnedCard = Card;
            Card = null;
            UpdateInterfaceObject();
            return returnedCard;
        }

        public override void ReturnCard(Deck deck)
        {
            if (!IsEmpty)
            {
                deck.ReturnCard(TakeCard());
                UpdateInterfaceObject();
            }
        }

        public void UpdateInterfaceObject()
        {
            if (CardObject != null)
            {
                CardObject.UpdateImage(Card);
            }
        }
    }
}
