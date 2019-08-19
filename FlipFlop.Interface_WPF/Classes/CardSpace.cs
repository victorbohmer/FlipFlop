using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlipFlop.Interface_WPF.Classes
{
    public class CardSpace
    {
        public Button WPFButton { get; protected set; }
        public Image WPFImage { get; protected set; }
        public Card Card { get; protected set; }
        public MainWindow MainWindow { get; protected set; }
        public ImageSource NoCardImage;
        public string Name { get { return WPFButton.Name; } }
        public bool IsEmpty ()
        {
            return Card == null;
        }
        protected void UpdateImage()
        {
            if (IsEmpty())
                WPFImage.Source = NoCardImage;
            else
                WPFImage.Source = (ImageSource)MainWindow.FindResource(Card.ToString());
        }

        public void PlaceCard(Card card)
        {
            Card = card;
            UpdateImage();
        }
        public Card TakeCard()
        {
            Card returnedCard = Card;
            Card = null;

            try
            {
                UpdateImage();
            }
            catch (System.IO.FileNotFoundException)
            {
                //Catches failure to update picture when called from project missing PresentationFramework
                // e.g. test project
            }

            return returnedCard;
        }

    }
}
