using FlipFlop.Interface_WPF.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.RecordKeeping
{
    class MatchRecord
    {
        public string Winner_Name { get; }
        public int Winner_Score { get; }
        public string Loser_Name { get; }
        public int Loser_Score { get; }
        public bool AIMatch { get; }
        public string MatchLength { get; }
        public string Date { get; }
        public MatchRecord(List<Player> sortedPlayers, string aiName)
        {
            Winner_Name = GetPlayerOrAIName(sortedPlayers[0], aiName);
            Winner_Score = sortedPlayers[0].Score;

            Loser_Name = GetPlayerOrAIName(sortedPlayers[1], aiName);
            Loser_Score = sortedPlayers[1].Score;

            AIMatch = GameMode.AI;
            MatchLength = GameMode.GameLength;
            Date = DateTime.Today.ToString("yyyy-MM-dd");
        }

        [JsonConstructor]
        public MatchRecord(string winner_Name, int winner_Score, string loser_Name, int loser_Score, bool aIMatch, string matchLength, string date)
        {
            Winner_Name = winner_Name;
            Winner_Score = winner_Score;
            Loser_Name = loser_Name;
            Loser_Score = loser_Score;
            AIMatch = aIMatch;
            MatchLength = matchLength;
            Date = date;
        }

        private string GetPlayerOrAIName(Player player, string aiName)
        {
            if (player.Id == 2 && GameMode.AI == true)
                return aiName;
            else
                return player.Name;
        }

        public override string ToString()
        {
            //Expected output
            //"2019-08-15: Victor won against Aziraphale (AI) with 17 points against 8"
            return $"{Date}: {Winner_Name} won against {Loser_Name} with {Winner_Score} points against {Loser_Score}";
        }
    }
}
