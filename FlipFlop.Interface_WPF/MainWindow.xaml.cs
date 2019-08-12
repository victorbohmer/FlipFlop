using FlipFlop.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipFlop.Interface_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EngineInterface EI = new EngineInterface();
        Button SelectedCard;
        public MainWindow()
        {
            InitializeComponent();
            EI.SetupFirstGame(this);
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            var keyPressed = e.Key;

            switch (keyPressed)
            {
                case Key.Escape:
                    DeselectCard();
                    break;
                case Key.Enter:
                    TryNextRound();
                    break;
                case Key.D1:
                    SetSelectedCardByKey(1);
                    break;
                case Key.D2:
                    SetSelectedCardByKey(2);
                    break;
                case Key.D3:
                    SetSelectedCardByKey(3);
                    break;
                case Key.D4:
                    SetSelectedCardByKey(4);
                    break;
                case Key.D5:
                    SetSelectedCardByKey(5);
                    break;
                case Key.NumPad1:
                    SelectBoardSpaceByKey(3, 1);
                    break;
                case Key.NumPad2:
                    SelectBoardSpaceByKey(3, 2);
                    break;
                case Key.NumPad3:
                    SelectBoardSpaceByKey(3, 3);
                    break;
                case Key.NumPad4:
                    SelectBoardSpaceByKey(2, 1);
                    break;
                case Key.NumPad5:
                    SelectBoardSpaceByKey(2, 2);
                    break;
                case Key.NumPad6:
                    SelectBoardSpaceByKey(2, 3);
                    break;
                case Key.NumPad7:
                    SelectBoardSpaceByKey(1, 1);
                    break;
                case Key.NumPad8:
                    SelectBoardSpaceByKey(1, 2);
                    break;
                case Key.NumPad9:
                    SelectBoardSpaceByKey(1, 3);
                    break;
                default:
                    break;
            }
        }

        private void TryNextRound()
        {
            if (NextRoundPopup.IsOpen)
            {
                NextRoundPopup.IsOpen = false;
                EI.ShowHand();
            }
        }

        private void SelectBoardSpaceByKey(int yPosition, int xPosition)
        {
            if (SelectedCard != null)
            {
                string spaceName = $"Played_Card_{yPosition}_{xPosition}";
                EI.PlayCard((Button)FindName(spaceName), SelectedCard);
            }
        }

        private void SetSelectedCardByKey(int cardIndex)
        {
            string cardName = $"Player_{EI.ActivePlayer}_Card_{cardIndex}";
            SetSelectedCard((Button)FindName(cardName));
        }

        private void PlayerCardClick(object sender, RoutedEventArgs e)
        {
            SetSelectedCard((Button)sender);
        }

        private void SetSelectedCard(Button sender)
        {
            if (SelectedCard != null)
            {
                DeselectCard();
            }
            SelectedCard = sender;
            SelectedCard.Background = (Brush)new BrushConverter().ConvertFrom("#5D82C7");
        }

        public void DeselectCard()
        {
            SelectedCard.Background = (Brush)new BrushConverter().ConvertFrom("#77CBD2");
            SelectedCard = null;
        }

        private void BoardSpaceClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCard != null)
            {
                Button clickedSpace = (Button)sender;
                EI.PlayCard(clickedSpace, SelectedCard);
            }
        }

        public Image GetImage(Button button)
        {
            return (Image)FindName(button.Name + "_Image");
        }

        private void NextRoundClick(object sender, RoutedEventArgs e)
        {
            NextRoundPopup.IsOpen = false;
            EI.ShowHand();
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            NewGamePopup.IsOpen = false;
            EI.SetupNewGame();
        }

    }
}
