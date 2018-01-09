using FunTimer.Lib.Models;
using System.Collections.Generic;

namespace FunTimer.Lib.Data
{
    public interface IDataLayer
    {
        void SaveTimeRecord(TimeRecord incoming);
        void SaveTimeRecordList(List<TimeRecord> incoming);
        List<TimeRecord> GetAllTimeRecords();
    }
}
