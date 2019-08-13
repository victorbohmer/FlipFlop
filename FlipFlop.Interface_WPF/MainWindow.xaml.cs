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
using FlipFlop.Interface_WPF.Classes;

namespace FlipFlop.Interface_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameEngine GE;
        public MainWindow()
        {
            InitializeComponent();
            GE = new GameEngine(this);
            GE.SetupFirstGame();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            Key keyPressed = e.Key;

            switch (keyPressed)
            {
                case Key.Escape:
                    GE.DeselectPlayerCard();
                    break;
                case Key.Enter:
                    TryGoToNextRound();
                    break;
                case Key.D1: case Key.D2: case Key.D3: case Key.D4: case Key.D5:
                    SelectPlayerCardByKey(keyPressed.ToString()[1]);
                    break;
                case Key.NumPad1: case Key.NumPad2: case Key.NumPad3: case Key.NumPad4: case Key.NumPad5: case Key.NumPad6: case Key.NumPad7: case Key.NumPad8: case Key.NumPad9:
                    SelectBoardSpaceByKey(keyPressed.ToString()[6]);
                    break;
                default:
                    break;
            }
        }

        

        private void NextRoundClick(object sender, RoutedEventArgs e)
        {
            NextRoundPopup.IsOpen = false;
            GE.ShowHand();
        }
        private void TryGoToNextRound()
        {
            if (NextRoundPopup.IsOpen)
            {
                NextRoundPopup.IsOpen = false;
                GE.ShowHand();
            }
            else if (NewGamePopup.IsOpen)
            {
                NewGamePopup.IsOpen = false;
                GE.SetupNewGame();
            }
        }

        private void PlayerCardClick(object sender, RoutedEventArgs e)
        {
            Button pressedButton = (Button)sender;
            GE.SelectPlayerCard(pressedButton.Name);
        }
        private void SelectPlayerCardByKey(char cardIndex)
        {
            string cardName = $"Player_{GE.ActivePlayer.Id}_Card_{cardIndex}";
            GE.SelectPlayerCard(cardName);
        }
        private void BoardSpaceClick(object sender, RoutedEventArgs e)
        {
            Button boardSpace = (Button)sender;
            GE.PlayCard(boardSpace.Name);
        }

        private void SelectBoardSpaceByKey(char boardSpaceIndex)
        {
            string spaceName = $"Played_Card_{boardSpaceIndex}";
            GE.PlayCard(spaceName);
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            NewGamePopup.IsOpen = false;
            GE.SetupNewGame();
        }
        internal void ShowNextRoundPopup()
        {
            NextRoundPopupText.Content = $"Player {GE.ActivePlayer.Id}'s turn!";

            NextRoundPopup.VerticalOffset = GE.ActivePlayer.Id == 1 ? -268 : 268;
            NextRoundPopup.IsOpen = true;
        }
        internal void ShowNewGamePopup(int score)
        {
            NewGamePopupText.Text = $"Player {GE.ActivePlayer.Id} controlled {GE.Board.WinnersControlledSpaces()} spaces and got {score} points!";

            NewGamePopup.IsOpen = true;
            NewGamePopup.VerticalOffset = GE.ActivePlayer.Id == 1 ? -268 : 268;
        }
        internal void ShowMatchEndPopup()
        {
            MatchEndPopupText.Text = $"Player {GE.ActivePlayer.Id} won with {GE.ActivePlayer.Score} points!";
            MatchEndPopup.IsOpen = true;
        }
        public void UpdateScoreBox()
        {
            TextBlock scoreBox = (TextBlock)FindName($"Player_{GE.ActivePlayer.Id}_Score");
            scoreBox.Text = GE.ActivePlayer.Score.ToString();
        }
        internal void SetBackgroundColorDark(PlayerCard selectedCard)
        {
            selectedCard.WPFButton.Background = (Brush)new BrushConverter().ConvertFrom("#5D82C7");
        }

        internal void SetBackgroundColorLight(PlayerCard selectedCard)
        {
            selectedCard.WPFButton.Background = (Brush)new BrushConverter().ConvertFrom("#77CBD2");
        }
    }
}
