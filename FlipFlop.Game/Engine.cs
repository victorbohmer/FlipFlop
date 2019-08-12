using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FlipFlop.Game
{
    public class Engine
    {
        Deck Deck = new Deck();
        Player Player1 = new Player();
        Player Player2 = new Player();
        public string Peek()
        {

            return Deck.GetRandomCard().ToString();

        }

        public void DrawNewHands()
        {
            foreach (Player player in new[] { Player1, Player2 })
            {
                ClearHand(player);
                DrawToFull(player);
            }

        }

        private void DrawToFull(Player player)
        {
            for (int i = player.Hand.Count; i < 5; i++)
            {
                player.Hand.Add(Deck.Draw());
            }
        }

        public List<string> GetCardNames(int playerIndex)
        {
            Player player = new Player();
            switch (playerIndex)
            {
                case 1:
                    player = Player1;
                    break;
                case 2:
                    player = Player2;
                    break;
                default:
                    break;
            }

            return player.Hand.Select(x => x.ToString()).ToList();

        }

        private void ClearHand(Player player)
        {
            foreach (Card card in player.Hand)
            {
                Deck.CardList.Add(card);
                player.Hand.Remove(card);
            }
        }
    }
}
