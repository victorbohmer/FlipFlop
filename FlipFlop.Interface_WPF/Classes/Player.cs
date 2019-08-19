using System;
using System.Collections.Generic;
using System.Text;

namespace FlipFlop.Interface_WPF.GameClasses
{
    public class Player
    {
        public int Id { get;}
        public List<PlayerCardSpace> Hand { get; private set; } = new List<PlayerCardSpace>();
        public Deck Deck { get;}
        public int Score { get; set; }
        public string Name { get; set; }

        public Player(Deck deck, MainWindow mainWindow, int id)
        {
            Deck = deck;
            Id = id;
            CreateHand(mainWindow);
            Name = $"Player {id}";
        }
        public Player(Deck deck, int id)
        {
            //Creates player that's not linked to WPF for use in tests or with other interfaces
            Deck = deck;
            Id = id;
            CreateHand();
            Name = $"Player {id}";
        }

        private void CreateHand(MainWindow mainWindow)
        {
            for (int cardIndex = 1; cardIndex <= 5; cardIndex++)
            {
                Hand.Add(new PlayerCardSpace(Id, cardIndex, mainWindow));
            }
        }
        private void CreateHand()
        {
            for (int cardIndex = 1; cardIndex <= 5; cardIndex++)
            {
                Hand.Add(new PlayerCardSpace(cardIndex));
            }
        }

        public void DrawNewHand()
        {
            Hand.ForEach(x => x.DrawNew(Deck));
        }

        public void ShowHand()
        {
            Hand.ForEach(x => x.ShowCard());
        }

        public void HideHand()
        {
            Hand.ForEach(x => x.HideCard());
        }
    }
}
