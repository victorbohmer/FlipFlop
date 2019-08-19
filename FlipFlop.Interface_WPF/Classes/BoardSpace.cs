using FlipFlop.Interface_WPF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlipFlop.Interface_WPF.GameClasses
{
    public class BoardCard
    {
        public Card Card { get; protected set; }
        private WPFCardObject CardSpace;
        public int Index { get; set; }
        public int Owner { get; set; }
        public bool IsEmpty { get { return Card == null; } }
        public string Name { get { return CardSpace.WPFButton.Name; } }

        public BoardCard(int spaceIndex, MainWindow mainWindow)
        {

            Index = spaceIndex;

            string buttonName = $"Played_Card_{spaceIndex}";
            CardSpace = new WPFCardObject(buttonName, mainWindow, false);

        }
        public BoardCard(int spaceIndex)
        {
            Index = spaceIndex;
        }

        public void PlaceCard(Card card)
        {
            Card = card;
            CardSpace.UpdateImage(Card);
        }
        public Card TakeCard()
        {
            Card returnedCard = Card;
            Card = null;
            CardSpace.UpdateImage(Card);

            return returnedCard;
        }

        public void ReturnCard(Deck deck)
        {
            if (!IsEmpty)
            {
                Owner = 0;
                deck.ReturnCard(TakeCard());
            }
        }

        public void ChangeOwner(int playerId)
        {
            Owner = playerId;
            CardSpace.RotateCard(playerId);
        }

        public void ResetColor()
        {
            CardSpace.SetColor(WPFColor.Grid);
        }

        public void SetColorFlipped()
        {
            CardSpace.SetColor(WPFColor.Popup);
        }
    }
}
