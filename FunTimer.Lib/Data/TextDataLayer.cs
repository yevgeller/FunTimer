using FunTimer.Lib;
using FunTimer.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FunTimer.Lib.Data
{
    public class TextDataLayer : IDataLayer
    {
        public TextDataLayer(string fileName)
        {
            this.fileName = fileName;

        }

        private void CreateFileIfItDoesNotExist()
        {

        }

        string fileName;

        public List<TimeRecord> GetAllTimeRecords()
        {
            List<TimeRecord> results = new List<TimeRecord>();
            using (StreamReader sr = new StreamReader(fileName, Encoding.ASCII))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    List<String> timeRecordPieces = line
                        .Split(new[] { '^' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    TimeRecordTypeEnum trte;
                    bool flag = Enum.TryParse(timeRecordPieces[0], out trte);

                    TimeRecord tr = new TimeRecord
                    {
                        TimeRecordType = flag ? trte : TimeRecordTypeEnum.WorkTimePeriod,
                        StartTime = Convert.ToDateTime(timeRecordPieces[1]),
                        EndTime = Convert.ToDateTime(timeRecordPieces[2])

                    };
                    results.Add(tr);
                }
            }
            return results;
        }

        public void SaveTimeRecord(TimeRecord incoming)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.ASCII))
            {
                sw.WriteLine(incoming.ConvertToString());
            }
        }

        public void SaveTimeRecordList(List<TimeRecord> incoming)
        {
            foreach(TimeRecord tr in incoming)
            {
                SaveTimeRecord(tr);
            }
        }
    }
}
