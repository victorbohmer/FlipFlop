using System;
using System.Collections.Generic;
using System.Text;

namespace FlipFlop.Interface_WPF.Classes
{
    public class Deck
    {
        readonly Random random = new Random();
        public List<Card> CardList { get; set; }

        public Deck()
        {
            CardList = new List<Card>();
            for (int i = 1; i <= 13; i++)
            {
                for (int suit = 0; suit < 4; suit++)
                {
                    CardList.Add(new Card (i,suit));
                }
            }
        }
        
        public bool EnoughCardsLeft()
        {
            return CardList.Count > GameMode.GameEndDeckSize;
        }

        public Card GetRandomCard()
        {
            return CardList[random.Next(CardList.Count)];
        }

        internal Card Draw()
        {
            Card drawnCard = GetRandomCard();
            CardList.Remove(drawnCard);
            return drawnCard;
        }

        internal void ReturnCard(Card card)
        {
            CardList.Add(card);
        }
    }
}
