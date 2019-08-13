using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlipFlop.Game
{
    public class GameEngine
    {
        
        public string Peek()
        {
            return Deck.GetRandomCard().ToString();
        }

        public void DrawNewHands()
        {
            foreach (Player player in Players)
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
            Player player = Players[playerIndex - 1];

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

        public void PlayCard(string cardName, int activePlayerId)
        {
            Player activePlayer = Players[activePlayerId-1];

            Card playedCard = activePlayer.Hand.Single(x => x.ToString() == cardName);

            PlayedCards.Add(playedCard);
            activePlayer.Hand.Remove(playedCard);

        }

        public int CardsLeft()
        {
            return Deck.CardList.Count();
        }

        public void RemovePlayedCards(int numberOfCardsToRemove)
        {
            for (int i = 0; i < numberOfCardsToRemove; i++)
            {
                PlayedCards.RemoveAt(random.Next(PlayedCards.Count));
            }
        }

        public void ShuffleBackIntoDeck()
        {

            Deck.CardList.AddRange(PlayedCards);
            PlayedCards = new List<Card>();
        }

        public bool EnoughCardsLeft()
        {
            return Deck.CardList.Count >= 26;
        }
    }
}
