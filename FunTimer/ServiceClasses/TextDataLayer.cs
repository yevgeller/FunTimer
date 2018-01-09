using FunTimer.Lib.Data;
using FunTimer.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Storage;

namespace FunTimer.Lib
{
    public class TextDataLayer : IDataLayer
    {
        public TextDataLayer(string fileName)
        {
            this.fileName = fileName;
            CreateFileIfItDoesNotExist();
        }

        private async void CreateFileIfItDoesNotExist()
        {
            StorageFile f = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);


            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile databaseFile = await storageFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);
            }
            catch { } //Ignore if exists? -- figure this out.
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
