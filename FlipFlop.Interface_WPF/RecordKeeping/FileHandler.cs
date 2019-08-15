using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipFlop.Interface_WPF.RecordKeeping
{
    class FileHandler
    {
        readonly string filePath = @"C:\Users\Victor Böhmer\Documents\FlipFlop\records.json";

        internal void SaveMatchRecord(MatchRecord record)
        {
            List<MatchRecord> recordList = GetRecordsFromFile();
            recordList.Add(record);

            SaveRecordsToFile(recordList);  
        }

        private void SaveRecordsToFile(List<MatchRecord> recordList)
        {
            string jsonOutput = JsonConvert.SerializeObject(recordList);

            System.IO.File.WriteAllText(filePath, jsonOutput);
        }

        public List<MatchRecord> GetRecordsFromFile()
        {
            try
            {
                string jsonInput = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<MatchRecord>>(jsonInput)
                                      ?? new List<MatchRecord>();
            }
            catch (System.IO.FileNotFoundException)
            {
                return new List<MatchRecord>();
            }
        }

    }
}
