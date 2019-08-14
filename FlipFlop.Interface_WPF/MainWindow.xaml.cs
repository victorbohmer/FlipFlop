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
using FlipFlop.Interface_WPF.Enums;

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
            WindowState = WindowState.Maximized;
            GE = new GameEngine(this);
            GE.SetupFirstGame();
        }
        public void SetupNewMatch()
        {
            GE.CleanUpAfterMatch();
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
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                    SelectPlayerCardByKey(keyPressed.ToString()[1]);
                    break;
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    SelectBoardSpaceByKey(keyPressed.ToString()[6]);
                    break;
                default:
                    break;
            }
        }

        private void NextRoundClick(object sender, RoutedEventArgs e)
        {
            NextRoundPopup.IsOpen = false;
            GE.StartRound();
        }
        private void TryGoToNextRound()
        {
            if (NextRoundPopup.IsOpen)
            {
                NextRoundPopup.IsOpen = false;
                GE.StartRound();
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
            GE.TryToPlayCard(boardSpace.Name);
        }

        private void SelectBoardSpaceByKey(char boardSpaceIndex)
        {
            string spaceName = $"Played_Card_{boardSpaceIndex}";
            GE.TryToPlayCard(spaceName);
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            NewGamePopup.IsOpen = false;
            GE.SetupNewGame();
        }

        private void NewMatchClick(object sender, RoutedEventArgs e)
        {
            MatchEndPopup.IsOpen = false;
            SetupNewMatch();
        }

        private void QuitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        internal void ShowNextRoundPopup()
        {
            NextRoundPopupText.Content = $"Player {GE.ActivePlayer.Id}'s turn!";

            NextRoundPopup.VerticalOffset = GE.ActivePlayer.Id == 1 ? -268 : 268;
            NextRoundPopup.IsOpen = true;
        }
        internal void ShowNewGamePopup(int score)
        {
            NewGamePopupText.Text = $"Player {GE.ActivePlayer.Id} controlled {(score + 9) / 2} spaces and got {score} {(score == 1 ? "point" : "points")}!";

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
        internal void UpdateDeckSize(int count)
        {
            Deck_Size.Text = count.ToString();
        }
        internal void SetBackgroundColor(Button button, WPFColor color)
        {
            button.Background = (SolidColorBrush)FindResource(color.ToString());
        }

        //

        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsPopup.IsOpen = true;
        }
        private void AIModeOnClick(object sender, RoutedEventArgs e)
        {
            SetBackgroundColor(AIModeOff, WPFColor.Background);
            SetBackgroundColor(AIModeOn, WPFColor.BackgroundDark);
            GameMode.AIModeOn();
        }
        private void AIModeOffClick(object sender, RoutedEventArgs e)
        {
            SetBackgroundColor(AIModeOn, WPFColor.Background);
            SetBackgroundColor(AIModeOff, WPFColor.BackgroundDark);
            GameMode.AIModeOff();
        }
        private void GameLengthShortClick(object sender, RoutedEventArgs e)
        {
            SetBackgroundColor(GameLengthLong, WPFColor.Background);
            SetBackgroundColor(GameLengthShort, WPFColor.BackgroundDark);
            GameMode.GameLengthShort();
        }
        private void GameLengthLongClick(object sender, RoutedEventArgs e)
        {
            SetBackgroundColor(GameLengthShort, WPFColor.Background);
            SetBackgroundColor(GameLengthLong, WPFColor.BackgroundDark);
            GameMode.GameLengthLong();
        }
        private void ExitSettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsPopup.IsOpen = false;
        }
        
    }
}
