using System;
using System.Collections.Generic;
using System.Text;

namespace FlipFlop.Interface_WPF.Classes
{
    public class Player
    {
        public int Id { get;}
        public List<PlayerCard> Hand { get; private set; } = new List<PlayerCard>();
        public Deck Deck { get;}
        public int Score { get; internal set; }

        public Player(Deck deck, MainWindow mainWindow, int id)
        {
            Deck = deck;
            Id = id;
            CreateHand(mainWindow);
        }

        private void CreateHand(MainWindow mainWindow)
        {
            for (int cardIndex = 1; cardIndex <= 5; cardIndex++)
            {
                Hand.Add(new PlayerCard(Id, cardIndex, mainWindow));
            }
        }

        internal void DrawNewHand()
        {
            Hand.ForEach(x => x.DrawNew(Deck));
        }

        internal void ShowHand()
        {
            Hand.ForEach(x => x.ShowCard());
        }

        internal void HideHand()
        {
            Hand.ForEach(x => x.HideCard());
        }
    }
}
