

using FlipFlop.Interface_WPF.Enums;

namespace FlipFlop.Interface_WPF.GameClasses
{
    public class PlayerCard
    {
        public Card Card { get; protected set; }
        private WPFCardObject CardSpace;
        public bool IsEmpty { get { return Card == null; } }
        public int Index { get; }
        public string Name { get { return CardSpace.WPFButton.Name; } }

        public PlayerCard(int playerId, int cardIndex, MainWindow mainWindow)
        {
            Index = cardIndex;

            string buttonName = $"Player_{playerId}_Card_{cardIndex}";
            CardSpace = new WPFCardObject(buttonName, mainWindow, true);
        }
        public PlayerCard(int cardIndex)
        {
            //Creates player card that's not linked to WPF for use in tests or with other interfaces
            Index = cardIndex;
        }
        public void DrawNew(Deck deck)
        {
            if (!IsEmpty)
                deck.ReturnCard(TakeCard());

            Card = deck.Draw();
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

            return returnedCard;
        }

        public void ShowCard()
        {
            CardSpace.UpdateImage(Card);
        }
        public void HideCard()
        {
            CardSpace.HideImage();
        }

        public bool CanBeSelected()
        {
            return CardSpace.ImageShowing();
        }

        public void Selected()
        {
            CardSpace.SetColor(WPFColor.GridDark);
        }

        public void Unselected()
        {
            CardSpace.SetColor(WPFColor.BackgroundDark);
        }
    }
}
