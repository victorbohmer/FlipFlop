

using FlipFlop.Interface_WPF.Classes_WPF;
using FlipFlop.Interface_WPF.Enums;

namespace FlipFlop.Interface_WPF.GameClasses
{
    public class PlayerCard : CardSpaceWPF
    {
        public string Name { get { return CardObject.WPFButton.Name; } }

        public PlayerCard(int playerId, int cardIndex, MainWindow mainWindow)
        {
            Index = cardIndex;

            string buttonName = $"Player_{playerId}_Card_{cardIndex}";
            CardObject = new WPFCardObject(buttonName, mainWindow, true);
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

        public void ShowCard()
        {
            CardObject.UpdateImage(Card);
        }
        public void HideCard()
        {
            CardObject.HideImage();
        }

        public bool CanBeSelected()
        {
            return CardObject.ImageShowing();
        }

        public void Selected()
        {
            CardObject.SetColor(WPFColor.GridDark);
        }

        public void Unselected()
        {
            CardObject.SetColor(WPFColor.BackgroundDark);
        }
    }
}
