using FlipFlop.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        Button selectedButton;
        public MainWindow()
        {
            InitializeComponent();
            EI.SetupGame(this);

        }

        private void PlayerCardClick(object sender, RoutedEventArgs e)
        {

            SetSelectedCard((Button)sender);
        }

        private void SetSelectedCard(Button sender)
        {
            if (selectedButton != null)
            {
                selectedButton.Background = new SolidColorBrush(Colors.AliceBlue);
            }
            selectedButton = sender;
            selectedButton.Background = new SolidColorBrush(Colors.MidnightBlue);
        }

        private void BoardSpaceClick(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                Button clickedSpace = (Button)sender;
                EI.PlayCard(clickedSpace, ref selectedButton);
            }
        }

        public Image GetImage(Button button)
        {
            return (Image)FindName(button.Name + "_Image");
        }

    }
}
