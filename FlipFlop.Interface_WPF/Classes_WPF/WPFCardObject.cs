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
    public class WPFCardObject
    {
        public Button WPFButton { get; protected set; }
        public Image WPFImage { get; protected set; }
        public MainWindow MainWindow { get; protected set; }
        public ImageSource NoCardImage;
        public WPFCardObject(string buttonName, MainWindow mainWindow, bool playerCard)
        {
            MainWindow = mainWindow;

            string imageName = buttonName + "_Image";

            WPFButton = (Button)MainWindow.FindName(buttonName);
            WPFImage = (Image)MainWindow.FindName(imageName);

            if (playerCard)
            {
                NoCardImage = (ImageSource)MainWindow.FindResource("card back");

            }
        }
        
        public void UpdateImage(Card card)
        {
            if (card == null)
                WPFImage.Source = NoCardImage;
            else
                WPFImage.Source = (ImageSource)MainWindow.FindResource(card.ToString());
        }

        internal void HideImage()
        {
            WPFImage.Source = NoCardImage;
        }

        internal void RotateCard(int playerId)
        {
            WPFButton.RenderTransform = new RotateTransform(playerId == 1 ? 0 : 90);
        }

        internal bool ImageShowing()
        {
            return WPFImage.Source != NoCardImage;
        }

        internal void SetColor(WPFColor color)
        {
            MainWindow.SetBackgroundColor(WPFButton, color);
        }

    }
}
