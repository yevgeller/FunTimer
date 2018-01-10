using FunTimer.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FunTimer.Lib.Data
{
    public class TextDataLayerFactory : IDataLayerAsync
    {
        /*
         * The idea here was to initialize a data layer class,
         * and provide the file name as parameter. 
         * It backfired with using async methods of checking for file existence
         * and the need to await. I tried using the Factory pattern to use
         * the static class initializer, but it did not work either.
         * I need to get smarter on async/await to figure this out.
         */

        static string fileName;

        public static string FileName { get; set; }

        //public string FileName { get; set; }
        public static TextDataLayerFactory CreateAsync(string fileName)
        {
            var ret = new TextDataLayerFactory();
            FileName = fileName;
            return ret.InitializeAsync(fileName).Result;
        }

        private async Task<TextDataLayerFactory> InitializeAsync(string fileName)
        {
            await CreateFileIfItDoesNotExist(fileName);
            return this;
        }

        //public TextDataLayer()//string fileName)
        //{
        //    //this.fileName = fileName;
        //    //await CreateFileIfItDoesNotExist();
        //}

        private static async Task CreateFileIfItDoesNotExist(string fileName)
        {
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            if (item == null)
            {
                StorageFolder sf = ApplicationData.Current.LocalFolder;
                await sf.CreateFileAsync(fileName);
            }
        }


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

        public async Task<List<TimeRecord>> GetAllTimeRecordsAsync()
        {
            StorageFolder sf = ApplicationData.Current.LocalFolder;
            StorageFile dbFile = await sf.GetFileAsync(fileName);
            string text = await FileIO.ReadTextAsync(dbFile);

            return new List<TimeRecord>();
        }

        public async Task SaveTimeRecordAsync(TimeRecord incoming)
        {
            StorageFolder sf = ApplicationData.Current.LocalFolder;
            StorageFile dbFile = await sf.GetFileAsync(fileName);
            await FileIO.WriteTextAsync(dbFile, incoming.ConvertToString());
        }

        public async Task SaveTimeRecordListAsync(List<TimeRecord> incoming)
        {
            foreach(TimeRecord tr in incoming)
            {
                await SaveTimeRecordAsync(tr);
            }
        }
    }
}
