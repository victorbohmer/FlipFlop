using FlipFlop.Interface_WPF.Classes;
using FlipFlop.Interface_WPF.Classes_WPF;
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
    public class BoardCardSpace : CardSpaceWPF
    {
        public int Owner { get; set; }
        public string Name { get { return CardObject.WPFButton.Name; } }

        public BoardCardSpace(int spaceIndex, MainWindow mainWindow)
        {

            Index = spaceIndex;

            string buttonName = $"Played_Card_{spaceIndex}";
            CardObject = new WPFCardObject(buttonName, mainWindow, false);

        }
        public BoardCardSpace(int spaceIndex)
        {
            Index = spaceIndex;
        }

        public void ChangeOwner(int playerId)
        {
            Owner = playerId;
            CardObject.RotateCard(playerId);
        }

        public void ResetColor()
        {
            CardObject.SetColor(WPFColor.Grid);
        }

        public void SetColorFlipped()
        {
            CardObject.SetColor(WPFColor.Popup);
        }
    }
}
