using FunTimer.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunTimer.Lib.Data
{
    public interface IDataLayerAsync
    {
        Task<List<TimeRecord>> GetAllTimeRecordsAsync();
        Task SaveTimeRecordAsync(TimeRecord incoming);
        Task SaveTimeRecordListAsync(List<TimeRecord> incoming);
    }
}
